using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Data;
using MiniHelpDesk.Services.TicketManagement.Core.Interfaces;
using MiniHelpDesk.Services.TicketManagement.Core;
using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Tickets;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Helpers;
using System.Text.Json;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Extensions;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace MiniHelpDesk.Services.TicketManagement.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketController : ControllerBase
    {
        public ITicketRepository _ticketRepository;
        private IUrlHelper _urlHelper;
        IPropertyMappingService _propertyMappingService;
        ITypeHelperService _typeHelperService;
        private ILogger<TicketController> _logger;
        IMapper<IEnumerable<Ticket>, IEnumerable<TicketDto>> _ticketDtoListMapper;
        IMapper<Ticket, TicketDto> _ticketDtoMapper;
        IMapper<TicketForCreationDto, Ticket> _ticketCreationMapper;
        IDoubleMapper<TicketForUpdateDto, Ticket> _ticketToTicketForUpdateDtoMapper;
        public string MAIL_HOST { get; set; } = "mail";
        public int MAIL_PORT { get; set; } = 1025;
        public TicketController(ITicketRepository ticketRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            ILogger<TicketController> logger,
            IMapper<IEnumerable<Ticket>, IEnumerable<TicketDto>> ticketDtoListMapper,
            IMapper<Ticket, TicketDto> ticketDtoMapper,
            IMapper<TicketForCreationDto, Ticket> ticketCreationMapper,
            IDoubleMapper<TicketForUpdateDto, Ticket> ticketToTicketForUpdateDtoMapper)
        {
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository)) ;
            _urlHelper = urlHelper  ?? throw new ArgumentNullException(nameof(urlHelper)) ;
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService)) ;
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService)) ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)) ;
            _ticketDtoListMapper = ticketDtoListMapper ?? throw new ArgumentNullException(nameof(ticketDtoListMapper));
            _ticketDtoMapper = ticketDtoMapper ?? throw new ArgumentNullException(nameof(ticketDtoMapper));
            _ticketCreationMapper = ticketCreationMapper ?? throw new ArgumentNullException(nameof(ticketCreationMapper));
            _ticketToTicketForUpdateDtoMapper = ticketToTicketForUpdateDtoMapper ?? throw new ArgumentNullException(nameof(ticketToTicketForUpdateDtoMapper));
        }

        [HttpGet(Name = "GetTickets")]
        public async Task<IActionResult> GetTicketsAsync([FromQuery]TicketsResourceParameters ticketsResourceParameters)
        {
            //if (!_propertyMappingService.ValidMappingExistsFor<TicketDto, Ticket>(ticketsResourceParameters.OrderBy))
            //{
            //    return BadRequest();
            //}

            //if (!_typeHelperService.TypeHasProperties<TicketDto>(ticketsResourceParameters.Fields))
            //{
            //    return BadRequest();
            //}

            var ticketsFromRepo = await _ticketRepository.GetTickets(ticketsResourceParameters);

            var paginationData = new
            {
                totalCount = ticketsFromRepo.TotalCount,
                pageSize = ticketsFromRepo.PageSize,
                currentPage = ticketsFromRepo.CurrentPage,
                totalPages = ticketsFromRepo.TotalPages
            };

            var jsonData = JsonSerializer.Serialize(paginationData, paginationData.GetType(), new JsonSerializerOptions
            {
                WriteIndented = false
            });
            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationData));
            Response.Headers.Add("T-Pagination", jsonData);

            var tickets = _ticketDtoListMapper.Map(ticketsFromRepo);

            return Ok(tickets.ShapeData(ticketsResourceParameters.Fields));//.net 5 'te expando object desteği geliyor
            //return Ok(tickets);
        }

        [HttpGet("{id}", Name = "GetTicket")]
        public async Task<IActionResult> GetTicketAsync(int Id, [FromQuery] string fields)
        {
            string rtVal = System.Environment.MachineName + " Date:" + DateTime.Now.ToString().ToLower(System.Globalization.CultureInfo.InvariantCulture);
            return Ok(rtVal);

            if (!_typeHelperService.TypeHasProperties<TicketDto>(fields))
            {
                return BadRequest();
            }

            var ticketFromRepo = await _ticketRepository.GetTicketAsync(Id);
            if (ticketFromRepo == null)
            {
                return NotFound();
            }

            var ticketToReturn = _ticketDtoMapper.Map(ticketFromRepo);

            return Ok(ticketToReturn.ShapeData(fields));
            //return Ok(ticketToReturn);
        }


        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] TicketForCreationDto ticketForCreationDto)
        {
            if (ticketForCreationDto == null)
            {
                return BadRequest();
            }

            //_logger.LogWarning($"{JsonConvert.SerializeObject(ticketForCreationDto)}");

            

            _logger.LogWarning(" {CreateTicketObject}:", JsonSerializer.Serialize(ticketForCreationDto, ticketForCreationDto.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            }));

            var ticketEntity = _ticketCreationMapper.Map(ticketForCreationDto);

            _ticketRepository.AddTicket(ticketEntity);

            await _ticketRepository.SaveChangesAsync();

            var ticketToReturn = _ticketDtoMapper.Map(ticketEntity);

            return CreatedAtRoute("GetTicket", new { Id = ticketToReturn.Id }, ticketToReturn);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int Id)
        {

            var ticketEntity = await _ticketRepository.GetTicketAsync(Id);
            if (ticketEntity == null)
            {
                return NotFound();
            }

            _logger.LogWarning($"{ticketEntity.Id} is soft deleted");

            ticketEntity.IsDeleted = true;
            _ticketRepository.UpdateTicket(ticketEntity);
            await _ticketRepository.SaveChangesAsync();

            return NoContent();

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody]TicketForUpdateDto ticketForUpdateDto)
        {
            if (ticketForUpdateDto == null)
            {
                return BadRequest();
            }

            var ticketFromRepo = await _ticketRepository.GetTicketAsync(id);
            if (ticketFromRepo == null)
            {
                //var errorJSON = JsonConvert.SerializeObject(new
                //{
                //    errorMessage = $"HelpDesk Task does not exist : {id}"
                //});

                var errorJSON = JsonSerializer.Serialize($"Ticket Id does not exist : {id}", "".GetType(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                return new UnprocessableEntityObjectResult(errorJSON);
            }

            //_logger.LogWarning($"{JsonConvert.SerializeObject(ticketForUpdateDto)}");
            _logger.LogWarning(JsonSerializer.Serialize(ticketForUpdateDto, ticketForUpdateDto.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            }));
            
            ticketFromRepo = _ticketToTicketForUpdateDtoMapper.Map(ticketForUpdateDto, ticketFromRepo);

            _ticketRepository.UpdateTicket(ticketFromRepo);
            await _ticketRepository.SaveChangesAsync();

            var updatedticket = _ticketDtoMapper.Map(ticketFromRepo);

            return Ok(updatedticket);

        }


    }
}

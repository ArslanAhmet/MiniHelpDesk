using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Data;
using MiniHelpDesk.Services.TicketManagement.Core.Interfaces;
using MiniHelpDesk.Services.TicketManagement.Core;
using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Helpers;
using System.Text.Json;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Extensions;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Persons;

namespace MiniHelpDesk.Services.TicketManagement.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonController : ControllerBase
    {
        public IPersonRepository _personRepository;
        private IUrlHelper _urlHelper;
        ITypeHelperService _typeHelperService;
        private ILogger<PersonController> _logger;
        IMapper<IEnumerable<Person>, IEnumerable<PersonDto>> _personDtoListMapper;
        IMapper<Person, PersonDto> _personDtoMapper;
        IMapper<PersonForCreationDto, Person> _personCreationMapper;
        IDoubleMapper<PersonForUpdateDto, Person> _personToPersonForUpdateDtoMapper;
        public string MAIL_HOST { get; set; } = "mail";
        public int MAIL_PORT { get; set; } = 1025;
        public PersonController(IPersonRepository personRepository,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService,
            ILogger<PersonController> logger,
            IMapper<IEnumerable<Person>, IEnumerable<PersonDto>> personDtoListMapper,
            IMapper<Person, PersonDto> personDtoMapper,
            IMapper<PersonForCreationDto, Person> personCreationMapper,
            IDoubleMapper<PersonForUpdateDto, Person> personToPersonForUpdateDtoMapper)
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _personDtoListMapper = personDtoListMapper ?? throw new ArgumentNullException(nameof(personDtoListMapper));
            _personDtoMapper = personDtoMapper ?? throw new ArgumentNullException(nameof(personDtoMapper));
            _personCreationMapper = personCreationMapper ?? throw new ArgumentNullException(nameof(personCreationMapper));
            _personToPersonForUpdateDtoMapper = personToPersonForUpdateDtoMapper ?? throw new ArgumentNullException(nameof(personToPersonForUpdateDtoMapper));
        }

        [HttpGet(Name = "GetPersons")]
        public async Task<IActionResult> GetPersonsAsync([FromQuery] PersonsResourceParameters personsResourceParameters)
        {

            if (!_typeHelperService.TypeHasProperties<PersonDto>(personsResourceParameters.Fields))
            {
                return BadRequest();
            }

            var personsFromRepo = await _personRepository.GetPersons(personsResourceParameters);

            var paginationData = new
            {
                totalCount = personsFromRepo.TotalCount,
                pageSize = personsFromRepo.PageSize,
                currentPage = personsFromRepo.CurrentPage,
                totalPages = personsFromRepo.TotalPages
            };

            var jsonData = JsonSerializer.Serialize(paginationData, paginationData.GetType(), new JsonSerializerOptions
            {
                WriteIndented = false
            });

            Response.Headers.Add("T-Pagination", jsonData);

            var persons = _personDtoListMapper.Map(personsFromRepo);

            return Ok(persons.ShapeData(personsResourceParameters.Fields));//.net 5 'te expando object desteği geliyor

        }

        [HttpGet("{id}", Name = "GetPerson")]
        public async Task<IActionResult> GetPersonAsync(int Id, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<PersonDto>(fields))
            {
                return BadRequest();
            }

            var personFromRepo = await _personRepository.GetPersonAsync(Id);
            if (personFromRepo == null)
            {
                return NotFound();
            }

            var personToReturn = _personDtoMapper.Map(personFromRepo);

            return Ok(personToReturn.ShapeData(fields));
        }


        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] PersonForCreationDto personForCreationDto)
        {
            if (personForCreationDto == null)
            {
                return BadRequest();
            }

            _logger.LogWarning(" {CreatePersonObject}:", JsonSerializer.Serialize(personForCreationDto, personForCreationDto.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            }));

            var personEntity = _personCreationMapper.Map(personForCreationDto);

            _personRepository.AddPerson(personEntity);

            var personToReturn = _personDtoMapper.Map(personEntity);

            return CreatedAtRoute("GetPerson", new { Id = personToReturn.Id }, personToReturn);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int Id)
        {

            var personEntity = await _personRepository.GetPersonAsync(Id);
            if (personEntity == null)
            {
                return NotFound();
            }

            _logger.LogWarning($"{personEntity.Id} is soft deleted");

            personEntity.IsDeleted = true;
            _personRepository.UpdatePerson(personEntity);

            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonForUpdateDto personForUpdateDto)
        {
            if (personForUpdateDto == null)
            {
                return BadRequest();
            }


            var personFromRepo = await _personRepository.GetPersonAsync(id);
            if (personFromRepo == null)
            {
                var errorJSON = JsonSerializer.Serialize($"Person Id does not exist : {id}", "".GetType(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                return new UnprocessableEntityObjectResult(errorJSON);
            }

            _logger.LogWarning(JsonSerializer.Serialize(personForUpdateDto, personForUpdateDto.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            }));

            personFromRepo = _personToPersonForUpdateDtoMapper.Map(personForUpdateDto, personFromRepo);

            _personRepository.UpdatePerson(personFromRepo);


            var updatedperson = _personDtoMapper.Map(personFromRepo);

            return Ok(updatedperson);

        }


    }
}

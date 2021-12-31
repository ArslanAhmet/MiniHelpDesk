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
using MiniHelpDesk.Services.TicketManagement.Core.Models.Organizations;

namespace MiniHelpDesk.Services.TicketManagement.Controllers
{
    [ApiController]
    [Route("api/organizations")]
    public class OrganizationController : ControllerBase
    {
        public IOrganizationRepository _organizationRepository;
        private IUrlHelper _urlHelper;
        ITypeHelperService _typeHelperService;
        private ILogger<OrganizationController> _logger;
        IMapper<IEnumerable<Organization>, IEnumerable<OrganizationDto>> _organizationDtoListMapper;
        IMapper<Organization, OrganizationDto> _organizationDtoMapper;
        IMapper<OrganizationForCreationDto, Organization> _organizationCreationMapper;
        IDoubleMapper<OrganizationForUpdateDto, Organization> _organizationToOrganizationForUpdateDtoMapper;
        public string MAIL_HOST { get; set; } = "mail";
        public int MAIL_PORT { get; set; } = 1025;
        public OrganizationController(IOrganizationRepository organizationRepository,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService,
            ILogger<OrganizationController> logger,
            IMapper<IEnumerable<Organization>, IEnumerable<OrganizationDto>> organizationDtoListMapper,
            IMapper<Organization, OrganizationDto> organizationDtoMapper,
            IMapper<OrganizationForCreationDto, Organization> organizationCreationMapper,
            IDoubleMapper<OrganizationForUpdateDto, Organization> organizationToOrganizationForUpdateDtoMapper)
        {
            _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _organizationDtoListMapper = organizationDtoListMapper ?? throw new ArgumentNullException(nameof(organizationDtoListMapper));
            _organizationDtoMapper = organizationDtoMapper ?? throw new ArgumentNullException(nameof(organizationDtoMapper));
            _organizationCreationMapper = organizationCreationMapper ?? throw new ArgumentNullException(nameof(organizationCreationMapper));
            _organizationToOrganizationForUpdateDtoMapper = organizationToOrganizationForUpdateDtoMapper ?? throw new ArgumentNullException(nameof(organizationToOrganizationForUpdateDtoMapper));
        }

        [HttpGet(Name = "GetOrganizations")]
        public async Task<IActionResult> GetOrganizationsAsync([FromQuery] OrganizationsResourceParameters organizationsResourceParameters)
        {

            if (!_typeHelperService.TypeHasProperties<OrganizationDto>(organizationsResourceParameters.Fields))
            {
                return BadRequest();
            }

            var organizationsFromRepo = await _organizationRepository.GetOrganizations(organizationsResourceParameters);

            var paginationData = new
            {
                totalCount = organizationsFromRepo.TotalCount,
                pageSize = organizationsFromRepo.PageSize,
                currentPage = organizationsFromRepo.CurrentPage,
                totalPages = organizationsFromRepo.TotalPages
            };

            var jsonData = JsonSerializer.Serialize(paginationData, paginationData.GetType(), new JsonSerializerOptions
            {
                WriteIndented = false
            });

            Response.Headers.Add("T-Pagination", jsonData);

            var organizations = _organizationDtoListMapper.Map(organizationsFromRepo);

            return Ok(organizations.ShapeData(organizationsResourceParameters.Fields));//.net 5 'te expando object desteği geliyor

        }

        [HttpGet("{id}", Name = "GetOrganization")]
        public async Task<IActionResult> GetOrganizationAsync(int Id, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<OrganizationDto>(fields))
            {
                return BadRequest();
            }

            var organizationFromRepo = await _organizationRepository.GetOrganizationAsync(Id);
            if (organizationFromRepo == null)
            {
                return NotFound();
            }

            var organizationToReturn = _organizationDtoMapper.Map(organizationFromRepo);

            return Ok(organizationToReturn.ShapeData(fields));
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrganization([FromBody] OrganizationForCreationDto organizationForCreationDto)
        {
            if (organizationForCreationDto == null)
            {
                return BadRequest();
            }

            _logger.LogWarning(" {CreateOrganizationObject}:", JsonSerializer.Serialize(organizationForCreationDto, organizationForCreationDto.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            }));

            var organizationEntity = _organizationCreationMapper.Map(organizationForCreationDto);

            _organizationRepository.AddOrganization(organizationEntity);

            var organizationToReturn = _organizationDtoMapper.Map(organizationEntity);

            return CreatedAtRoute("GetOrganization", new { Id = organizationToReturn.Id }, organizationToReturn);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(int Id)
        {

            var organizationEntity = await _organizationRepository.GetOrganizationAsync(Id);
            if (organizationEntity == null)
            {
                return NotFound();
            }

            _logger.LogWarning($"{organizationEntity.Id} is soft deleted");

            organizationEntity.IsDeleted = true;
            _organizationRepository.UpdateOrganization(organizationEntity);

            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrganization(int id, [FromBody] OrganizationForUpdateDto organizationForUpdateDto)
        {
            if (organizationForUpdateDto == null)
            {
                return BadRequest();
            }


            var organizationFromRepo = await _organizationRepository.GetOrganizationAsync(id);
            if (organizationFromRepo == null)
            {
                var errorJSON = JsonSerializer.Serialize($"Organization Id does not exist : {id}", "".GetType(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                return new UnprocessableEntityObjectResult(errorJSON);
            }

            _logger.LogWarning(JsonSerializer.Serialize(organizationForUpdateDto, organizationForUpdateDto.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            }));

            organizationFromRepo = _organizationToOrganizationForUpdateDtoMapper.Map(organizationForUpdateDto, organizationFromRepo);

            _organizationRepository.UpdateOrganization(organizationFromRepo);


            var updatedorganization = _organizationDtoMapper.Map(organizationFromRepo);

            return Ok(updatedorganization);

        }


    }
}

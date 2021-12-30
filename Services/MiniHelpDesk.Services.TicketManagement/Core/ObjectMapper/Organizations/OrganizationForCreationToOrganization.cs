using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Organizations;

namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Organizations
{
    public class OrganizationForCreationToOrganization : IMapper<OrganizationForCreationDto, Organization>
    {
        public Organization Map(OrganizationForCreationDto input)
        {
            var entity = new Organization
            {
                Name = input.Name?.Trim(),
                IsDeleted = input.IsDeleted,
                Domain = input.Domain
            };
            return entity;
        }
    }
}

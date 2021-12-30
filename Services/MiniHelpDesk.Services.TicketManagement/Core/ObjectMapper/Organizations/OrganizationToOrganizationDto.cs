using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Organizations;

namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Organizations
{
    public class OrganizationToOrganizationDto : IMapper<Organization, OrganizationDto>
    {
        public OrganizationDto Map(Organization input)
        {
            var entity = new OrganizationDto
            {
                Id = input.Id,
                Name = input.Name?.Trim(),
                Domain = input.Domain,
                IsDeleted = input.IsDeleted
            };
            return entity;
        }
    }
}

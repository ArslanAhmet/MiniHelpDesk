using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Organizations;

namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Organizations
{
    public class OrganizationToOrganizationForUpdateDto : IDoubleMapper<OrganizationForUpdateDto, Organization>
    {
        public Organization Map(OrganizationForUpdateDto source, Organization destination)
        {
            destination.Name = source.Name?.Trim();
            destination.IsDeleted = source.IsDeleted;
            destination.Domain = source.Domain;
            return destination;
        }
    }
}

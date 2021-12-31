using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Helpers;
using System.Threading.Tasks;

namespace MiniHelpDesk.Services.TicketManagement.Infrastructure.Data
{
    public interface IOrganizationRepository
    {
        Task<PagedList<Organization>> GetOrganizations(OrganizationsResourceParameters organizationsResourceParameters);
        Task<Organization> GetOrganizationAsync(int id);
        void AddOrganization(Organization organization);
        void UpdateOrganization(Organization organization);
    }
}

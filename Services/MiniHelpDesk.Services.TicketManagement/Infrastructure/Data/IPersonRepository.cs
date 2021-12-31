using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Helpers;
using System.Threading.Tasks;

namespace MiniHelpDesk.Services.TicketManagement.Infrastructure.Data
{
    public interface IPersonRepository
    {
        Task<PagedList<Person>> GetPersons(PersonsResourceParameters personsResourceParameters);
        Task<Person> GetPersonAsync(int id);
        void AddPerson(Person person);
        void UpdatePerson(Person person);

    }
}

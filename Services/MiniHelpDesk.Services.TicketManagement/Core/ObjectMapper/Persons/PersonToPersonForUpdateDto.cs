using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Persons;

namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Persons
{
    public class PersonToPersonForUpdateDto : IDoubleMapper<PersonForUpdateDto, Person>
    {
        public Person Map(PersonForUpdateDto source, Person destination)
        {
            destination.Language = source.Language;
            destination.IsDeleted = source.IsDeleted;
            destination.Name = source.Name;
            destination.Email = source.Email;
            destination.Type = source.Type;
            destination.LatestUpdate = source.LatestUpdate;
            return destination;
        }
    }
}

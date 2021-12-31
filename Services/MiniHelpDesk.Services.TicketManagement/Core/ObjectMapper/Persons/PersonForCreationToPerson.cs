using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Persons;

namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Persons
{
    public class PersonForCreationToPerson : IMapper<PersonForCreationDto, Person>
    {
        public Person Map(PersonForCreationDto input)
        {
            var entity = new Person
            {
                Language = input.Language?.Trim(),
                Name = input.Name,
                Email = input.Email,
                Type = input.Type
            };
            return entity;
        }
    }
}

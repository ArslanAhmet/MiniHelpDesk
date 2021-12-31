using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Persons;

namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Persons
{
    public class PersonToPersonDto : IMapper<Person, PersonDto>
    {
        public PersonDto Map(Person input)
        {
            var entity = new PersonDto
            {
                Id = input.Id,
                Language = input.Language,
                Name = input.Name,
                Email = input.Email,
                Type = input.Type,
                LatestUpdate = input.LatestUpdate
            };
            return entity;
        }
    }
}

using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Persons;
using System.Collections.Generic;

namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Persons
{

    public class PersonListToPersonDtoList : IMapper<IEnumerable<Person>, IEnumerable<PersonDto>>
    {
        public IEnumerable<PersonDto> Map(IEnumerable<Person> entities)
        {
            var entityDtos = new List<PersonDto>();

            foreach (var entity in entities)
            {
                entityDtos.Add(
                    new PersonDto
                    {
                        Id = entity.Id,
                        Language = entity.Language,
                        Name = entity.Name,
                        Email = entity.Email,
                        Type = entity.Type,
                        LatestUpdate = entity.LatestUpdate
                    });
            }
            return entityDtos;
        }
    }
}

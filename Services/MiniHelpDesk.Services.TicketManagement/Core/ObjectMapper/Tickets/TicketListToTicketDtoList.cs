using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Tickets;
using System;
using System.Collections.Generic;


namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Tickets
{
    public class TicketListToTicketDtoList : IMapper<IEnumerable<Ticket>, IEnumerable<TicketDto>>
    {
        public IEnumerable<TicketDto> Map(IEnumerable<Ticket> entities)
        {
            var entityDtos = new List<TicketDto>();

            foreach (var entity in entities)
            {
                entityDtos.Add(
                    new TicketDto
                    {
                        Id = entity.Id,
                        //Name = entity.Name.Trim(),
                        Description = entity.Description?.Trim()
                    });
            }
            return entityDtos;
        }
    }
}

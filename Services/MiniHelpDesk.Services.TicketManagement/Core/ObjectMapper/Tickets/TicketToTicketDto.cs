using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Tickets;
using System;

namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Tickets
{
    public class TicketToTicketDto : IMapper<Ticket, TicketDto>
    {
        public TicketDto Map(Ticket input)
        {
            var entity = new TicketDto
            {
                Id = input.Id,
                Name = input.Name.Trim(),
                Description = input.Description?.Trim()
            };
            return entity;
        }
    }
}

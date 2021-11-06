using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Tickets;
using System;

namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Tickets
{
    public class TicketForCreationToTicket : IMapper<TicketForCreationDto, Ticket>
    {
        public Ticket Map(TicketForCreationDto input)
        {
            var entity = new Ticket
            {
                //Name = input.Name.Trim(),
                Description = input.Description?.Trim()
            };
            return entity;
        }
    }
}

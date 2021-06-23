
using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Tickets;

namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Tickets
{
    public class TicketToTicketForUpdateDto : IDoubleMapper<TicketForUpdateDto, Ticket>
    {
        public Ticket Map(TicketForUpdateDto source, Ticket destination)
        {
            destination.Name = source.Name.Trim();
            destination.Description = source.Description?.Trim();
            return destination;
        }
    }
}

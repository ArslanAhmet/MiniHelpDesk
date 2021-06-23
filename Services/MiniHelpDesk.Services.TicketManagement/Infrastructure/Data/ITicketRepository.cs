using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Helpers;
using System;
using System.Threading.Tasks;

namespace MiniHelpDesk.Services.TicketManagement.Infrastructure.Data
{
    public interface ITicketRepository
    {
        Task<PagedList<Ticket>> GetTickets(TicketsResourceParameters ticketsResourceParameters);
        Task<Ticket> GetTicketAsync(int id);
        Task<Ticket> CheckTicketExists(string id);
        void AddTicket(Ticket ticket);
        void UpdateTicket(Ticket ticket);
        bool TicketExists(int id);
        Task<bool> SaveChangesAsync();
    }
}

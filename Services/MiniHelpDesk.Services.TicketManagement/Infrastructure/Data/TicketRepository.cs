using Microsoft.EntityFrameworkCore;
using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Interfaces;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Tickets;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Extensions;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniHelpDesk.Services.TicketManagement.Infrastructure.Data
{
    public class TicketRepository : ITicketRepository
    {
        private TicketContext _context;
        IPropertyMappingService _propertyMappingService;
        
        public TicketRepository(TicketContext context, 
            IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
            
        }

        #region Ticket
        public async Task<PagedList<Ticket>> GetTickets(TicketsResourceParameters helpDeskTasksResourceParameters)
        {
            var collectionBeforePaging = _context.Tickets
                .ApplySort(helpDeskTasksResourceParameters.OrderBy, _propertyMappingService.GetPropertyMapping<TicketDto, Ticket>());

            if (!string.IsNullOrEmpty(helpDeskTasksResourceParameters.SearchQuery))
            {
                var searchQueryToWhereClause = helpDeskTasksResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging.Where(c =>
                c.Name.ToLowerInvariant().Contains(searchQueryToWhereClause)
               || c.Description.ToLowerInvariant().Contains(searchQueryToWhereClause)
                );
            }


            return await PagedList<Ticket>.Create(collectionBeforePaging
                , helpDeskTasksResourceParameters.PageNumber
                , helpDeskTasksResourceParameters.PageSize);
        }
        public void AddTicket(Ticket helpDeskTask)
        {
            _context.Tickets.Add(helpDeskTask);
        }

        public bool TicketExists(int Id)
        {
            return _context.Tickets.Any(a => a.Id.CompareTo(Id) == 0);
        }
        public void UpdateTicket(Ticket helpDeskTask)
        {
            // no code in this implementation
        }
         

        public async Task<Ticket> GetTicketAsync(int Id)
        {
            return await _context.Tickets
                .FirstOrDefaultAsync(a =>  Id.CompareTo(a.Id) == 0);
        }

        public async Task<Ticket> CheckTicketExists(string helpDeskTaskNo)
        {
            return await _context.Tickets
                .FirstOrDefaultAsync(k => k.Name == helpDeskTaskNo );
        }
        public async Task<bool> SaveChangesAsync()
        {    
            return (await _context.SaveChangesAsync() > 0);   
        }

         

        #endregion
    }
}

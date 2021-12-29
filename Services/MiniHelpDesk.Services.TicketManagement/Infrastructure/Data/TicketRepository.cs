using Dapper;
using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Interfaces;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Tickets;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Extensions;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Helpers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MiniHelpDesk.Services.TicketManagement.Infrastructure.Data
{
    public class TicketRepository : ITicketRepository
    {
        private string _connectionString;

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_connectionString);
            }
        }
        public TicketRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<PagedList<Ticket>> GetTickets(TicketsResourceParameters ticketsResourceParameters)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var collection = await dbConnection
                    .QueryAsync<Ticket>("SELECT *, count(\"Id\") OVER() AS FullCount FROM public.\"Tickets\" ORDER BY @OrderBy LIMIT  @PageSize offset @Offset", 
                    new {
                        OrderBy = ticketsResourceParameters.OrderBy,
                        PageSize = ticketsResourceParameters.PageSize, 
                        Offset = (ticketsResourceParameters.PageNumber - 1) * ticketsResourceParameters.PageSize 
                    }) ;

                var fullCount = collection.Count() == 0 ? 0 : collection.FirstOrDefault().FullCount;

                PagedList<Ticket> pagedList = new PagedList<Ticket>(collection.ToList(), 
                    fullCount, 
                    ticketsResourceParameters.PageNumber, 
                    ticketsResourceParameters.PageSize);
                
                return pagedList;
            }

            
        }

        public Task<Ticket> GetTicketAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void AddTicket(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket> CheckTicketExists(string id)
        {
            throw new NotImplementedException();
        }

        

        

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public bool TicketExists(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateTicket(Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }
}

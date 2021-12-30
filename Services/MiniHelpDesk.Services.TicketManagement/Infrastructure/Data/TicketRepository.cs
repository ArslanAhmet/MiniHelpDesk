using Dapper;
using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Helpers;
using Npgsql;
using System;
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

        public async Task<Ticket> GetTicketAsync(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                var records = await dbConnection
                    .QueryAsync<Ticket>("select * from public.\"Tickets\"  where \"Id\"= @id and \"IsDeleted\" = false", 
                    new { id =  id })
                    .ConfigureAwait(false);

                return records.SingleOrDefault();
            }
        }

        public void AddTicket(Ticket ticket)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                ticket.Created = DateTime.Now;
                ticket.Id = dbConnection.ExecuteScalar<int>("insert into public.\"Tickets\" " +
                    "(\"Description\",\"IsDeleted\",\"Created\"," +
                    "\"Subject\" , \"RequesterID\",\"AssigneeID\"," +
                    "\"Priority\" , \"OrganizationID\",\"SubmitterID\", " +
                    "\"Channel\" , \"BrandID\",\"TicketTypeID\", " +
                    "\"Satisfaction\" , \"DueDate\",\"SolvedDate\" " +
                    ") " +
                    "values(:Description, :IsDeleted, :Created, :Subject, :RequesterID, :AssigneeID," +
                    ":Priority, :OrganizationID, :SubmitterID,:Channel, :BrandID, :TicketTypeID," +
                    " :Satisfaction, :DueDate, :SolvedDate) RETURNING \"Id\" ", ticket);
                
            }
        }

        public void UpdateTicket(Ticket ticket)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                dbConnection.Execute("UPDATE \"Tickets\" Set" +
                    " \"Description\" = @Description, \"IsDeleted\" = @IsDeleted, \"Modified\" = @Modified ," +
                    " \"Subject\" = @Subject, \"RequesterID\" = @RequesterID, \"AssigneeID\" = @AssigneeID ," +
                    " \"Priority\" = @Priority, \"OrganizationID\" = @OrganizationID, \"SubmitterID\" = @SubmitterID ," +
                    " \"Channel\" = @Channel, \"BrandID\" = @BrandID, \"TicketTypeID\" = @TicketTypeID ," +
                    " \"Satisfaction\" = @Satisfaction, \"DueDate\" = @DueDate, \"SolvedDate\" = @SolvedDate " +
                    "where \"Id\"= @id ",
                 new
                 {
                     Description = ticket.Description,
                     IsDeleted = ticket.IsDeleted,
                     Modified = DateTime.Now,
                     Subject = ticket.Subject,
                     RequesterID = ticket.RequesterID,
                     AssigneeID = ticket.AssigneeID,
                     Priority = ticket.Priority,
                     OrganizationID = ticket.OrganizationID,
                     SubmitterID = ticket.SubmitterID,
                     Channel = ticket.Channel,
                     BrandID = ticket.BrandID,
                     TicketTypeID = ticket.TicketTypeID,
                     Satisfaction = ticket.Satisfaction,
                     DueDate = ticket.DueDate,
                     SolvedDate = ticket.SolvedDate,
                     id = ticket.Id
                 });
            }
        }
    }
}

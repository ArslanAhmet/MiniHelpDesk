using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Tickets;

namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Tickets
{
    public class TicketForCreationToTicket : IMapper<TicketForCreationDto, Ticket>
    {
        public Ticket Map(TicketForCreationDto input)
        {
            var entity = new Ticket
            {
                Description = input.Description?.Trim(),   
                IsDeleted = input.IsDeleted,
                Subject = input.Subject,
                RequesterID = input.RequesterID,
                AssigneeID = input.AssigneeID,
                Priority = input.Priority,
                OrganizationID = input.OrganizationID,
                SubmitterID = input.SubmitterID,
                Channel = input.Channel,
                BrandID = input.BrandID,
                TicketTypeID = input.TicketTypeID,
                Satisfaction = input.Satisfaction,
                DueDate = input.DueDate,
                SolvedDate = input.SolvedDate
            };
            return entity;
        }
    }
}

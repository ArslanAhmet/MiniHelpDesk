
using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Tickets;

namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Tickets
{
    public class TicketToTicketForUpdateDto : IDoubleMapper<TicketForUpdateDto, Ticket>
    {
        public Ticket Map(TicketForUpdateDto source, Ticket destination)
        {
            destination.Description = source.Description?.Trim();
            destination.IsDeleted = source.IsDeleted;
            destination.Subject = source.Subject;
            destination.RequesterID = source.RequesterID;
            destination.AssigneeID = source.AssigneeID;
            destination.Priority = source.Priority;
            destination.OrganizationID = source.OrganizationID;
            destination.SubmitterID = source.SubmitterID;
            destination.Channel = source.Channel;
            destination.BrandID = source.BrandID;
            destination.TicketTypeID = source.TicketTypeID;
            destination.Satisfaction = source.Satisfaction;
            destination.DueDate = source.DueDate;
            destination.SolvedDate = source.SolvedDate;
            return destination;
        }
    }
}

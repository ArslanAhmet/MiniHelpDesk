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
                Description = input.Description?.Trim(),
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

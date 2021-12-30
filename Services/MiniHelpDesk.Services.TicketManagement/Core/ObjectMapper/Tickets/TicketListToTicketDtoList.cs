using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Tickets;
using System;
using System.Collections.Generic;


namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Tickets
{
    public class TicketListToTicketDtoList : IMapper<IEnumerable<Ticket>, IEnumerable<TicketDto>>
    {
        public IEnumerable<TicketDto> Map(IEnumerable<Ticket> entities)
        {
            var entityDtos = new List<TicketDto>();

            foreach (var entity in entities)
            {
                entityDtos.Add(
                    new TicketDto
                    {
                        Id = entity.Id,
                        Description = entity.Description?.Trim(),
                        Subject = entity.Subject,
                        RequesterID = entity.RequesterID,
                        AssigneeID = entity.AssigneeID,
                        Priority = entity.Priority,
                        OrganizationID = entity.OrganizationID,
                        SubmitterID = entity.SubmitterID,
                        Channel = entity.Channel,
                        BrandID = entity.BrandID,
                        TicketTypeID = entity.TicketTypeID,
                        Satisfaction = entity.Satisfaction,
                        DueDate = entity.DueDate,
                        SolvedDate = entity.SolvedDate
                    });
            }
            return entityDtos;
        }
    }
}

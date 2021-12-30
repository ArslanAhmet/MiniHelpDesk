using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Organizations;
using System.Collections.Generic;

namespace MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Organizations
{
    public class OrganizationListToOrganizationDtoList : IMapper<IEnumerable<Organization>, IEnumerable<OrganizationDto>>
    {
        public IEnumerable<OrganizationDto> Map(IEnumerable<Organization> entities)
        {
            var entityDtos = new List<OrganizationDto>();

            foreach (var entity in entities)
            {
                entityDtos.Add(
                    new OrganizationDto
                    {
                        Id = entity.Id,
                        Name = entity.Name?.Trim(),
                        Domain = entity.Domain,
                        IsDeleted = entity.IsDeleted
                    });
            }
            return entityDtos;
        }
    }
}

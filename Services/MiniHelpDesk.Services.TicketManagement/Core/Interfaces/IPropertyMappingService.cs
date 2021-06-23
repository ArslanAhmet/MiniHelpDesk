using MiniHelpDesk.Services.TicketManagement.Core.Services;
using System.Collections.Generic;

namespace MiniHelpDesk.Services.TicketManagement.Core.Interfaces
{
    public interface IPropertyMappingService
    {
        bool ValidMappingExistsFor<TSource, TDestination>(string fields);
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}

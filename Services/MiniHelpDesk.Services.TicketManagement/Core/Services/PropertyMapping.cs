using MiniHelpDesk.Services.TicketManagement.Core.Interfaces;
using System.Collections.Generic;

namespace MiniHelpDesk.Services.TicketManagement.Core.Services
{
    public class PropertyMapping<TSource,TDestination> : IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> _mappingDictionary { get; private set; }
        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            _mappingDictionary = mappingDictionary;
        }
    }
}

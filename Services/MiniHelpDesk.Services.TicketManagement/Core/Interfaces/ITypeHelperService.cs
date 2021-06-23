using System;
using System.Collections.Generic;
using System.Text;

namespace MiniHelpDesk.Services.TicketManagement.Core.Interfaces
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}

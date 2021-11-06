using System;

namespace MiniHelpDesk.Services.TicketManagement.Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}

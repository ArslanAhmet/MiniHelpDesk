using System;

namespace MiniHelpDesk.Services.TicketManagement.Core.Entities
{
    public class Person : BaseEntity
    {
        public int FullCount { get; set; }
        public string Language { get; set; }
        public short Type { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime LatestUpdate { get; set; }       

    }
}

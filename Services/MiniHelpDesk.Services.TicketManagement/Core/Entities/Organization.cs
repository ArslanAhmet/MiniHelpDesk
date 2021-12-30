namespace MiniHelpDesk.Services.TicketManagement.Core.Entities
{
    public class Organization : BaseEntity
    {
        public int FullCount { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string Domain { get; set; }
    }
}

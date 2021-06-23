namespace MiniHelpDesk.Services.TicketManagement.Core.Entities
{
    public class Ticket : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}

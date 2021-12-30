namespace MiniHelpDesk.Services.TicketManagement.Core.Models.Organizations
{
    public class OrganizationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string Domain { get; set; }
    }
}

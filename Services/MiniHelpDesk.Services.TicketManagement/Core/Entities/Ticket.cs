using System;

namespace MiniHelpDesk.Services.TicketManagement.Core.Entities
{
    public class Ticket : BaseEntity
    {
        public int AssigneeID { get; set; }
        public int BrandID { get; set; }
        public string Channel { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsDeleted { get; set; }
        public int OrganizationID { get; set; }
        public int Priority { get; set; }
        public int RequesterID { get; set; }
        public int Satisfaction { get; set; }
        public DateTime? SolveDate { get; set; }
        public int Subject { get; set; }
        public int SubmitterID { get; set; }
        public int TicketTypeID { get; set; }
    }
}

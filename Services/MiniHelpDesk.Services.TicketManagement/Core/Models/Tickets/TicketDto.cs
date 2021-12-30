using System;
using System.Collections.Generic;
using System.Text;

namespace MiniHelpDesk.Services.TicketManagement.Core.Models.Tickets
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public int RequesterID { get; set; }
        public int AssigneeID { get; set; }
        public short Priority { get; set; }
        public int OrganizationID { get; set; }
        public int SubmitterID { get; set; }
        public short Channel { get; set; }
        public int BrandID { get; set; }
        public int TicketTypeID { get; set; }
        public short Satisfaction { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime SolvedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniHelpDesk.Services.TicketManagement.Core.Models.Tickets
{
    public abstract class TicketForManipulationDto
    {
        [Required(ErrorMessage = "Ticket name is required")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace MiniHelpDesk.Services.TicketManagement.Core.Models.Persons
{
    public class PersonForManipulationDto
    {
        public string Language { get; set; }
        public short Type { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public DateTime LatestUpdate { get; set; }
        public bool IsDeleted { get; set; }
    }
}

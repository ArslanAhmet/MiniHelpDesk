using System.ComponentModel.DataAnnotations;

namespace MiniHelpDesk.Services.TicketManagement.Core.Models.Organizations
{
    public abstract class OrganizationForManipulationDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "IsDeleted is required")]
        public bool IsDeleted { get; set; }
        public string Domain { get; set; }
    }
}

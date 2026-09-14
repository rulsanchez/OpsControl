using System.ComponentModel.DataAnnotations;

namespace OpsControl.Application.Incidents.DTOs{
    public class UpdateIncidentRequest
    {
        [Required]      
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}

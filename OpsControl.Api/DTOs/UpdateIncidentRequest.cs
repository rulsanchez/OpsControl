using System.ComponentModel.DataAnnotations;

namespace OpsControl.Api.DTOs
{
    public class UpdateIncidentRequest
    {
        [Required]      
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}

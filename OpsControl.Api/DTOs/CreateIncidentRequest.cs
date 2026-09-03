using System.ComponentModel.DataAnnotations;

namespace OpsControl.Api.DTOs
{
    public class CreateIncidentRequest
    {
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}

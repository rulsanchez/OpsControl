using OpsControl.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace OpsControl.Api.DTOs
{
    public class CreateIncidentRequest
    {
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public IncidentImpact? Impact { get; set; }

        [Required]
        public IncidentUrgency? Urgency { get; set; }
    }
}

using OpsControl.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace OpsControl.Application.Incidents.DTOs
{
    public class CreateIncidentRequest
    {
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        [EnumDataType(typeof(IncidentImpact))]
        public IncidentImpact? Impact { get; set; }

        [Required]
        [EnumDataType(typeof(IncidentUrgency))]
        public IncidentUrgency? Urgency { get; set; }
    }
}

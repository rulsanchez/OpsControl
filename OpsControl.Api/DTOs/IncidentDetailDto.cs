using OpsControl.Api.Models.Enums;

namespace OpsControl.Api.DTOs
{
    public class IncidentDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public IncidentStatus Status { get; set; }
        public string? Description { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public IncidentImpact Impact { get; set; }
        public IncidentUrgency Urgency { get; set; }
        public IncidentPriority Priority { get; set; }
    }
}

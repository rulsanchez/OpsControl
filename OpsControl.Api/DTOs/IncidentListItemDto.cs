using OpsControl.Api.Models.Enums;

namespace OpsControl.Api.DTOs
{
    public class IncidentListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public IncidentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public IncidentPriority Priority { get; set; }
    }
}

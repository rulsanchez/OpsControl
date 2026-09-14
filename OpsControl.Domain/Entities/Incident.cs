using OpsControl.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace OpsControl.Domain.Entities
{
    public class Incident
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; private set; } = string.Empty;
        public string? Description { get; set; }
        public IncidentStatus Status { get; private set; } = IncidentStatus.New;
        public DateTime CreatedAt { get; set; }
        public IncidentImpact Impact { get; private set; }
        public IncidentUrgency Urgency { get; private set; }
        /// <summary>
        /// Prioridad no la enviará el usuario, se calculárá con lógica
        /// </summary>

        public IncidentPriority Priority { get; private set; }
        public DateTime? ResolvedAt { get; private set; }

        /// <summary>
        /// calcula la prioridad 
        /// </summary>
        /// <param name="impact"></param>
        /// <param name="urgency"></param>
        /// <returns></returns>
        public static IncidentPriority CalculatePriority(IncidentImpact impact, IncidentUrgency urgency)
        {

            if (impact == IncidentImpact.High && urgency == IncidentUrgency.High)
                return IncidentPriority.Critical;
            if (impact == IncidentImpact.High || urgency == IncidentUrgency.High)
                return IncidentPriority.High;
            if (impact == IncidentImpact.Low && urgency == IncidentUrgency.Low)
                return IncidentPriority.Low;

            return IncidentPriority.Medium;
        }

        public bool TryStartWork()
        {
            if (Status == IncidentStatus.New)
            {
                Status = IncidentStatus.InProgress;
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool TryResolve()
        {
            if (Status != IncidentStatus.InProgress)
                return false;

            Status = IncidentStatus.Resolved;
            ResolvedAt = DateTime.UtcNow;
            return true;

        }



        public bool UpdateDetails(string title, string? description)
        {
            if (string.IsNullOrWhiteSpace(title))
                return false;
            Title = title;
            Description = description;
            return true;
        }

        public void ChangeImpactAndUrgency(IncidentImpact impact, IncidentUrgency urgency)
        {
            Impact = impact;
            Urgency = urgency;
            Priority = CalculatePriority(impact, urgency);
        }

    }
}


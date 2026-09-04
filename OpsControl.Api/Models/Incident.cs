using OpsControl.Api.Models.Enums;

namespace OpsControl.Api.Models
{
    public class Incident
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IncidentStatus Status { get; private set; } = IncidentStatus.New;
        public DateTime CreatedAt{ get; set; }
        public IncidentImpact Impact { get; set; }
        public IncidentUrgency Urgency { get; set; }
        public DateTime? ResolvedAt { get; private set; }
        /// <summary>
        /// Prioridad no la enviará el usuario, se calculárá con lógica
        /// </summary>
        public IncidentPriority Priority { get; set; }

        /// <summary>
        /// calcula la prioridad 
        /// </summary>
        /// <param name="impact"></param>
        /// <param name="urgency"></param>
        /// <returns></returns>
        public static IncidentPriority CalculatePriority(IncidentImpact impact,IncidentUrgency urgency)
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
            if(Status==IncidentStatus.New)
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


    }
}


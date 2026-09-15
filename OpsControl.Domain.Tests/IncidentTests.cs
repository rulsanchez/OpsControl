using OpsControl.Domain.Entities;
using OpsControl.Domain.Enums;
using Xunit;
namespace OpsControl.Domain.Tests
{
    public class IncidentTests
    {
        [Fact]
        public void HighImpactAndHighUrgency_ProducesCriticalPriority()
        {
            var incident = new Incident();

            incident.ChangeImpactAndUrgency(
                IncidentImpact.High,
                IncidentUrgency.High);

            Assert.Equal(IncidentPriority.Critical, incident.Priority);
        }
    }
}

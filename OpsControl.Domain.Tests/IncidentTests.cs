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
        [Fact]
        public void TryStartWork_WhenResolved_ReturnsFalseAndKeepsResolved()
        {
            // Preparar una incidencia resuelta
            var incident = new Incident();
            Assert.True(incident.TryStartWork());
            Assert.True(incident.TryResolve());

            // Intentar iniciarla otra vez
            var result = incident.TryStartWork();

            // Comprobar que rechaza el cambio y mantiene el estado
            Assert.False(result);
            Assert.Equal(IncidentStatus.Resolved, incident.Status);
        }
    }
}

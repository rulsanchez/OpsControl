using OpsControl.Application.Incidents.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpsControl.Application.Incidents.UseCases
{
    public enum StartIncidentResult
    {
        Success,
        NotFound,
        InvalidState
    }
    public class StartIncident
    {
        private readonly IIncidentRepository _incidentRepository;
        public StartIncident(IIncidentRepository incidentRepository)
        {
            _incidentRepository = incidentRepository;
        }
        public async Task<StartIncidentResult> ExecuteAsync(int id)
        {
            var startIncident = await _incidentRepository.GetByIdAsync(id);
            if (startIncident == null)
                return StartIncidentResult.NotFound;

            if (!startIncident.TryStartWork())
                return StartIncidentResult.InvalidState;

            await _incidentRepository.SaveChangesAsync();
            return StartIncidentResult.Success;





        }

    }
}

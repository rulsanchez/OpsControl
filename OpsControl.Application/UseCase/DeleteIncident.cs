using OpsControl.Application.Incidents.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpsControl.Application.Incidents.UseCases
{
    public class DeleteIncident
    {
        private readonly IIncidentRepository _incidentRepository;
        public DeleteIncident(IIncidentRepository incidentRepository)
        {
            _incidentRepository = incidentRepository;
        }

        public async Task<bool>ExecuteAsync(int id)
        {

            var incidentToDelete=await _incidentRepository.GetByIdAsync(id);

            if (incidentToDelete == null)
                return false;
             _incidentRepository.Remove(incidentToDelete);
            await _incidentRepository.SaveChangesAsync();
            return true;

        }
    }
}

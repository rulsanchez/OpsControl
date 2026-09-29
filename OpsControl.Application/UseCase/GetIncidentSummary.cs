using OpsControl.Application.Incidents.DTOs;
using OpsControl.Application.Incidents.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpsControl.Application.UseCase
{
    public class GetIncidentSummary
    {
        private readonly IIncidentRepository _incidentRepository;

        public GetIncidentSummary(IIncidentRepository incidenRepository)
        {
            _incidentRepository = incidenRepository;
                
        }

        public Task<IncidentSummaryResponse> ExecuteAsync()
        {
            return  _incidentRepository.GetSummaryAsync();
        
        }

    }
}

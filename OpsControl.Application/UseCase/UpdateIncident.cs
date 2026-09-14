using System;
using System.Collections.Generic;
using System.Text;
using OpsControl.Application.Incidents.DTOs;
using OpsControl.Application.Incidents.Repositories;


namespace OpsControl.Application.Incidents.UseCases
{
    public enum UpdateIncidentResult
    {
        Success,
        NotFound,
        InvalidTitle
    }
    public class UpdateIncident
    {
        private readonly  IIncidentRepository _repository;
        public UpdateIncident(IIncidentRepository  repository)
        {
            _repository = repository;            
        }

        public async Task<UpdateIncidentResult> ExecuteAsync(int id, UpdateIncidentRequest request)
        {
       
           var incidentToUpdate= await _repository.GetByIdAsync(id);
            if (incidentToUpdate==null)
                return UpdateIncidentResult.NotFound;

            if(incidentToUpdate.UpdateDetails(request.Title, request.Description)==false)
                return UpdateIncidentResult.InvalidTitle;

            await _repository.SaveChangesAsync();
            return UpdateIncidentResult.Success;

        }
    }
}

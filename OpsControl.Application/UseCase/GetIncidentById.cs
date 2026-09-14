using OpsControl.Application.Incidents.DTOs;
using OpsControl.Application.Incidents.Repositories;
namespace OpsControl.Application.Incidents.UseCases;

public class GetIncidentById
{
    private readonly IIncidentRepository _repository;

    public GetIncidentById(IIncidentRepository repository)
    {
        _repository = repository;
            
    }

    public async Task<IncidentDetailDto?> ExecuteAsync(int id)
    {
        var incident = await _repository.GetByIdAsync(id);
        if (incident==null)
        {
            return null;
        }

        IncidentDetailDto incidentDetailDto = new IncidentDetailDto()
        {
            Id = incident.Id,
            Description = incident.Description,
            Title = incident.Title ?? string.Empty,
            Impact = incident.Impact,
            CreatedAt = incident.CreatedAt,
            Priority = incident.Priority,
            ResolvedAt = incident.ResolvedAt,
            Status = incident.Status,
            Urgency = incident.Urgency

        };
        return incidentDetailDto;

        

    }
}
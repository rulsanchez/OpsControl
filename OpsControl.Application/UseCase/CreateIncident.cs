using OpsControl.Application.Incidents.DTOs;
using OpsControl.Application.Incidents.Repositories;
using OpsControl.Domain.Entities;
using OpsControl.Domain.Enums;

public class CreateIncident
{
    private readonly IIncidentRepository _repository;


    public CreateIncident(IIncidentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IncidentDetailDto?> ExecuteAsync(CreateIncidentRequest request)
    {
       
        Incident incident = new Incident
        {
            CreatedAt = DateTime.UtcNow
        };
        if (!incident.UpdateDetails(request.Title, request.Description))
        {
            return null;
        }

        incident.ChangeImpactAndUrgency(request.Impact.Value, request.Urgency.Value);
        await _repository.AddAsync(incident);

        await _repository.SaveChangesAsync();

        return new IncidentDetailDto
        {
            Id = incident.Id,
            Title = incident.Title ?? string.Empty,
            Description = incident.Description,
            CreatedAt = incident.CreatedAt,
            Status = incident.Status,
            Priority = incident.Priority,
            Impact = incident.Impact,
            Urgency = incident.Urgency,
            ResolvedAt = incident.ResolvedAt
        };
    }
}
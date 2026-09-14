using OpsControl.Application.Incidents.Repositories;

namespace OpsControl.Application.Incidents.UseCases;

public enum ResolveIncidentResult
{
    Success,
    NotFound,
    InvalidState
}

public class ResolveIncident
{
    private readonly IIncidentRepository _repository;

    public ResolveIncident(IIncidentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResolveIncidentResult> ExecuteAsync(int id)
    {
        var incident = await _repository.GetByIdAsync(id);

        if (incident == null)
        {
            return ResolveIncidentResult.NotFound;
        }

        var resolved = incident.TryResolve();

        if (!resolved)
        {
            return ResolveIncidentResult.InvalidState;
        }

        await _repository.SaveChangesAsync();

        return ResolveIncidentResult.Success;
    }
}
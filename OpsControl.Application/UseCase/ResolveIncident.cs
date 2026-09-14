using OpsControl.Application.Incidents.Repositories;

namespace OpsControl.Application.Incidents.UseCases;

public enum ResolveIncidentResult
{
    Success,
    NotFound,
    AlreadyResolved
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

        var resolved = incident.Resolve();

        if (!resolved)
        {
            return ResolveIncidentResult.AlreadyResolved;
        }

        await _repository.SaveChangesAsync();

        return ResolveIncidentResult.Success;
    }
}
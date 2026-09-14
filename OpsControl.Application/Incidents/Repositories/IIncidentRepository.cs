using OpsControl.Application.Incidents.DTOs;
using OpsControl.Domain.Entities;
using OpsControl.Domain.Enums;

namespace OpsControl.Application.Incidents.Repositories;

public interface IIncidentRepository
{
    Task<Incident?> GetByIdAsync(int id);

    Task SaveChangesAsync();

    Task<(IReadOnlyList<Incident> Items, int TotalCount)> GetPagedAsync(IncidentStatus? status, int page, int pageSize);

    Task AddAsync(Incident incident);

    void Remove(Incident incident);

}
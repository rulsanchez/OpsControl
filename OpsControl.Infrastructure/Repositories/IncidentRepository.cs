using Microsoft.EntityFrameworkCore;
using OpsControl.Application.Incidents.DTOs;
using OpsControl.Application.Incidents.Repositories;
using OpsControl.Domain.Entities;
using OpsControl.Domain.Enums;
using OpsControl.Infrastructure.Data;

namespace OpsControl.Infrastructure.Incidents.Repositories;

public class IncidentRepository : IIncidentRepository
{
    private readonly AppDbContext _context;

    public IncidentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Incident?> GetByIdAsync(int id)
    {
        return await _context.Incidents
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }


    public async Task<(IReadOnlyList<Incident> Items, int TotalCount)> GetPagedAsync(IncidentStatus? status, int page, int pageSize)
    {
        IQueryable<Incident> query = _context.Incidents.AsNoTracking();

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        int totalCount = await query.CountAsync();

        List<Incident> items = await query.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(Incident incident)
    {
        await _context.Incidents.AddAsync(incident);
    }

    public void Remove(Incident incident)
    {
         _context.Incidents.Remove(incident);
    }
}

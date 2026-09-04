using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpsControl.Api.Data;
using OpsControl.Api.DTOs;
using OpsControl.Api.Models;
using OpsControl.Api.Models.Enums;
namespace OpsControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentsController : ControllerBase
    {
        readonly AppDbContext _context;

        public IncidentsController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] IncidentStatus? status,[FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Incident> query = _context.Incidents;
            if (page < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest();
            }
            if (!status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }
           

            var totalCount = await query.CountAsync();
            query = query.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize);
            var items = await query
                .Select(incident => new IncidentListItemDto
                {
                    Id = incident.Id,
                    Title = incident.Title ?? string.Empty,
                    Status = incident.Status,
                    CreatedAt = incident.CreatedAt,
                    Priority=Incident.CalculatePriority(incident.Impact,incident.Urgency)
                })
                .ToListAsync();

            IncidentPageResponse response = new IncidentPageResponse
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return Ok(response);


        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateIncidentRequest request)
        {
            var incidentToUpdate = await _context.Incidents.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (incidentToUpdate != null)
            {
                incidentToUpdate.Title = request.Title;
                incidentToUpdate.Description = request.Description;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var incidentToDelete = await _context.Incidents.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (incidentToDelete != null)
            {
                _context.Incidents.Remove(incidentToDelete);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound();
            }



        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var incident = await _context.Incidents.Where(x => x.Id == id)
                .Select(cont=>new IncidentDetailDto 
                {
                     Id=cont.Id,
                     Status=cont.Status,
                     Title=cont.Title?? string.Empty,
                     Description=cont.Description,
                     CreatedAt=cont.CreatedAt,
                     Impact=cont.Impact,
                     Urgency=cont.Urgency,
                     Priority=cont.Priority,
                     ResolvedAt=cont.ResolvedAt
                     

                })
                .FirstOrDefaultAsync();

            return incident != null ? Ok(incident) : NotFound();

        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncidentRequest request)
        {
            Incident incident = new Incident()
            {
                Title = request.Title,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                Impact = request.Impact.Value,
                Urgency=request.Urgency.Value,
                Priority=Incident.CalculatePriority(request.Impact.Value,request.Urgency.Value)

            };
            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = incident.Id }, incident);
        }


        [HttpPut("{id:int}/start")]
        public async Task<IActionResult> StartWork(int id)
        {
            var getIncident = await _context.Incidents.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (getIncident==null)
            {
                return NotFound();
            }
            if(!getIncident.TryStartWork())
            {
                return Conflict();
            }
            else
            {
                
                await _context.SaveChangesAsync();
                return NoContent();
            }
        
        }
        [HttpPut("{id:int}/resolve")]

        public async Task<IActionResult> Resolve(int id)
        {
            var getIncident = await _context.Incidents.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (getIncident == null)
            {
                return NotFound();
            }
            if (!getIncident.TryResolve())
            {
                return Conflict();
            }
            else
            {

                await _context.SaveChangesAsync();
                return NoContent();
            }

        }

    }
}
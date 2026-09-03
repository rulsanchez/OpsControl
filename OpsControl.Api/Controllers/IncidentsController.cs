using Microsoft.AspNetCore.Mvc;
using OpsControl.Api.Models;
using OpsControl.Api.Data;
using Microsoft.EntityFrameworkCore;
using OpsControl.Api.DTOs;
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
        private List<Incident> _incidents = new List<Incident>()
         {
                new Incident
                {
                    Id = 1,
                    Title = "aplicacion devuevle error",
                    Description = "blablabla",
                    Status = "InProgress",
                    CreatedAt=DateTime.UtcNow

        },
                 new Incident
                {
                    Id = 2,
                    Title = "Las consultas de SQL Server tardan demasiado",
                    Description = " ccccccblablabla",
                    Status = "InProgress",
                    CreatedAt=DateTime.UtcNow

        }

                 //Server=localhost\SQLEXPRESS01;Database=master;Trusted_Connection=True;

    };
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? status,[FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Incident> query = _context.Incidents;
            if (page < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest();
            }
         
            if (string.IsNullOrEmpty(status))
            {
                query = query.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize);
                var allIncidents = await query.ToListAsync();
                return Ok(allIncidents);
            }
            else
            {
                query = query.Where(x => x.Status == status);
                query= query.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize);

                var filterIncidents = await query.ToListAsync();
                return Ok(filterIncidents);
            }


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
            var incident = await _context.Incidents.Where(x => x.Id == id).FirstOrDefaultAsync();

            return incident != null ? Ok(incident) : NotFound();

        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncidentRequest request)
        {
            Incident incident = new Incident()
            {
                Title = request.Title,
                Description = request.Description,
                Status = "new",
                CreatedAt = DateTime.UtcNow

            };
            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = incident.Id }, incident);



        }


    }
}
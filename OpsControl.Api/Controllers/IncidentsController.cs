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
        public IActionResult Get([FromQuery] string? status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return Ok(_incidents);
            }
            else
            {
                return Ok(_incidents.Where(x => string.Equals(x.Status, status, StringComparison.OrdinalIgnoreCase)).ToList());
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
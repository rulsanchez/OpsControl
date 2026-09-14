using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpsControl.Application.Incidents.DTOs;
using OpsControl.Application.Incidents.UseCases;
using OpsControl.Application.UseCase;
using OpsControl.Domain.Enums;
namespace OpsControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentsController : ControllerBase
    {
        private readonly ResolveIncident _resolveIncident;
        private readonly GetIncidentById _getIncidentById;
        private readonly GetIncidents _getIncidents;
        private readonly CreateIncident _createIncident;
        private readonly UpdateIncident _updateIncident;
        private readonly DeleteIncident _deleteIncident;

        private readonly StartIncident _startIncident;

        public IncidentsController(ResolveIncident resolveIncident, GetIncidentById getIncidentById, GetIncidents getIncidents
            , CreateIncident createIncident, UpdateIncident updateIncident, DeleteIncident deleteIncident, StartIncident startIncident)
        {
            _resolveIncident = resolveIncident;
            _getIncidentById = getIncidentById;
            _getIncidents = getIncidents;
            _createIncident = createIncident;
            _updateIncident = updateIncident;
            _deleteIncident = deleteIncident;
            _startIncident = startIncident;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] IncidentStatus? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {

            if (page < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest();
            }

            var response = await _getIncidents.ExecuteAsync(status, page, pageSize);
            return Ok(response);


        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateIncidentRequest request)
        {

            var response = await _updateIncident.ExecuteAsync(id, request);
            if (response==UpdateIncidentResult.InvalidTitle)
                return BadRequest("El título no puede estar vacío.");
            if (response == UpdateIncidentResult.NotFound)
                return NotFound();
            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _deleteIncident.ExecuteAsync(id);

            if (deleted)
                return NoContent();
            return NotFound();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var incident = await _getIncidentById.ExecuteAsync(id);
            if (incident == null)
            {
                return NotFound();
            }
            return Ok(incident);

        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncidentRequest request)
        {


            var createdIncident = await _createIncident.ExecuteAsync(request);
            if (createdIncident == null)
            {
                return BadRequest("El título no puede estar vacío.");
            }
            return CreatedAtAction(nameof(GetById), new { id = createdIncident.Id }, createdIncident);

        }


        [HttpPut("{id:int}/start")]
        public async Task<IActionResult> StartWork(int id)
        {
            
            var startWork = await _startIncident.ExecuteAsync(id);
            if (startWork == StartIncidentResult.NotFound)
                return NotFound();
            if (startWork == StartIncidentResult.InvalidState)
                return Conflict();
            return NoContent();


        }
        [HttpPut("{id:int}/resolve")]

        public async Task<IActionResult> Resolve(int id)
        {
            var result = await _resolveIncident.ExecuteAsync(id);
            if (result == ResolveIncidentResult.NotFound)
            {
                return NotFound();
            }
            if (result == ResolveIncidentResult.InvalidState)
            {
                return Conflict("La incidencia ya estaba resuelta");
            }

            return NoContent();


        }

    }
}
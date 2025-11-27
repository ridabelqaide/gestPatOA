using System;
using PATOA.CORE.Entities;
using PATOA.APPLICATION.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PATOA.APPLICATION.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace PATOA.WEBAPI.Controllers
{
    [ApiController]
    [Route("api/Official")]
    public class OfficialController : ControllerBase
    {
        private readonly IOfficialService _service;

        public OfficialController(IOfficialService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Official>>> GetAll()
        {
            var list = await _service.GetAllOfficialsAsync();
            return Ok(list);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<Official>>> GetPaged(
            [FromQuery] string? genre,
            [FromQuery] string? fonction,
            [FromQuery] string? service,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            var result = await _service.GetPagedOfficialsAsync(genre, fonction, service, page, pageSize);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var official = await _service.GetOfficialByIdAsync(id);
            if (official == null)
                return NotFound(new { message = $"Aucun official trouvé avec l'ID {id}." });
            return Ok(official);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Official official)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateOfficialAsync(official);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                new { message = $"Official '{created.FirstName} {created.LastName}' créé avec succès.", data = created });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Official official)
        {
            try
            {
                var updated = await _service.UpdateOfficialAsync(id, official);
                return Ok(new { message = $"Official '{official.FirstName} {official.LastName}' mis à jour avec succès." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deletedMessage = await _service.DeleteOfficialAsync(id);
                return Ok(new { message = deletedMessage });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

}
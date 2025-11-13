using Microsoft.AspNetCore.Mvc;
using PATOA.CORE.Entities;
using PATOA.INFRA.Repositories;
using PATOA.APPLICATION.Interfaces;
using PATOA.CORE.Interfaces;
using PATOA.APPLICATION.Services;
using PATOA.APPLICATION.DTOs;
namespace PATOA.WebAPI.Controllers
{
    [ApiController]
    [Route("api/PrivatePat")]
    public class PrivatePatController : ControllerBase
    {
        private readonly IPrivatePatService _service;

        public PrivatePatController(IPrivatePatService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrivatePat>>> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }


        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<PrivatePat>>> GetPaged(
    [FromQuery] string? typeAr,
    [FromQuery] string? locationAr,
    [FromQuery] string? registrationNumber,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetPagedAsync(typeAr, locationAr, registrationNumber, page, pageSize);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PrivatePat privatePat)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateAsync(privatePat);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PrivatePat privatePat)
        {
            var result = await _service.UpdateAsync(id, privatePat);
            if (!result) return NotFound();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PATOA.CORE.Entities;
using PATOA.INFRA.Repositories;
using PATOA.APPLICATION.Interfaces;
using PATOA.CORE.Interfaces;
using PATOA.APPLICATION.Services;

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
        public async Task<ActionResult<IEnumerable<PublicPat>>> GetAll([FromQuery] string? registrationNumber)
        {
            var list = await _service.GetAllAsync(registrationNumber);
            return Ok(list);
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

            var created = await _service.CreateAsync(privatePat);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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

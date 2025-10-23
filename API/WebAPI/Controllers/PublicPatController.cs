using Microsoft.AspNetCore.Mvc;
using PATOA.CORE.Entities;
using PATOA.APPLICATION.Interfaces;
using System;
using System.Threading.Tasks;

namespace PATOA.WebAPI.Controllers
{
    [ApiController]
    [Route("api/PublicPat")]
    public class PublicPatController : ControllerBase
    {
        private readonly IPublicPatService _service;

        public PublicPatController(IPublicPatService service)
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
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PublicPat publicPat)
        {
            await _service.CreateAsync(publicPat);
            return CreatedAtAction(nameof(GetById), new { id = publicPat.Id }, publicPat);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PublicPat publicPat)
        {
            var result = await _service.UpdateAsync(id, publicPat);
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

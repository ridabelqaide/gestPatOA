using Microsoft.AspNetCore.Mvc;
using PATOA.CORE.Entities;
using PATOA.INFRA.Repositories;

namespace PATOA.WebAPI.Controllers
{
    [Route("api/engins")]
    [ApiController]
    public class EnginsController : ControllerBase
    {
        private readonly EnginRepository _enginRepository;

        public EnginsController(EnginRepository enginRepository)
        {
            _enginRepository = enginRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Engin>>> GetAll()
        {
            return Ok(await _enginRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Engin>> GetById(Guid id)
        {
            var engin = await _enginRepository.GetByIdAsync(id);
            if (engin == null)
                return NotFound();
            return Ok(engin);
        }

        [HttpPost]
        public async Task<ActionResult<Engin>> Create(Engin engin)
        {
            var newEngin = await _enginRepository.AddAsync(engin);
            return CreatedAtAction(nameof(GetById), new { id = newEngin.Id }, newEngin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Engin engin)
        {
            if (id != engin.Id)
                return BadRequest();

            await _enginRepository.UpdateAsync(engin);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _enginRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

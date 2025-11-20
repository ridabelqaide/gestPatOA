using Microsoft.AspNetCore.Mvc;
using PATOA.APPLICATION.DTOs;
using PATOA.APPLICATION.Interfaces;
using PATOA.CORE.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PATOA.WebAPI.Controllers
{
    [Route("api/EnginType")]
    [ApiController]
    public class EnginTypeController : ControllerBase
    {
        private readonly IEnginTypeService _service;

        public EnginTypeController(IEnginTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnginType>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<EnginType>>> Search(
            [FromQuery] string? name,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetPagedAsync(name, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<EnginType>> GetByCode(string code)
        {
            var type = await _service.GetByCodeAsync(code);
            if (type == null) return NotFound();
            return Ok(type);
        }

        [HttpPost]
        public async Task<ActionResult<EnginType>> Create([FromBody] EnginType model)
        {
            var created = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetByCode), new { code = created.Code }, created);
        }

        [HttpPut("{code}")]
        public async Task<ActionResult<EnginType>> Update(string code, [FromBody] EnginType model)
        {
            var updated = await _service.UpdateAsync(code, model);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{code}")]
        public async Task<ActionResult> Delete(string code)
        {
            var result = await _service.DeleteAsync(code);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}

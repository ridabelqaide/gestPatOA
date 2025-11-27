using System;
using Microsoft.AspNetCore.Mvc;
using PATOA.APPLICATION.Interfaces;
using PATOA.CORE.Entities;

namespace PATOA.WEBAPI.Controllers
{
    [ApiController]
    [Route("api/Affectation")]

    public class AffectationController : ControllerBase
    {
        private readonly IAffectationService _affectationService;

        public AffectationController(IAffectationService affectationService)
        {
            _affectationService = affectationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _affectationService.GetAllAsync();
            return Ok(list);
        }
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
       [FromQuery] Guid? officialId,
       [FromQuery] Guid? enginId,
       [FromQuery] DateTime? startDate,
       [FromQuery] int page = 1,
       [FromQuery] int pageSize = 5)
        {
            var result = await _affectationService.GetPagedAffectationsAsync(officialId, enginId, startDate, page, pageSize);

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _affectationService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Affectation affectation)
        {
            var created = await _affectationService.AddAsync(affectation);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Affectation affectation)
        {
            affectation.Id = id; 
            var updated = await _affectationService.UpdateAsync(affectation);
            if (updated == null) return NotFound();
            return Ok(updated);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _affectationService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return Ok();
        }

    }

}
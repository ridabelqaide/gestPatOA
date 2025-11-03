using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PATOA.APPLICATION.DTOs;
using PATOA.CORE.Entities;
using PATOA.INFRA.Data;
using PATOA.INFRA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PATOA.WebAPI.Controllers
{
    [Route("api/engins")]
    [ApiController]
    public class EnginsController : ControllerBase
    {
        private readonly EnginRepository _enginRepository;
        private readonly ApplicationDbContext _context;

        public EnginsController(EnginRepository enginRepository, ApplicationDbContext context)
        {
            _enginRepository = enginRepository;
            _context = context;
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

        [HttpGet("last-insurance")]
        public async Task<ActionResult<IEnumerable<InsuranceDto>>> GetLastInsurancePerEngin(
      [FromQuery] string? search = null
  )
        {
            var query = _context.Engins
                .Select(e => new
                {
                    Engin = e,
                    LastInsurance = _context.Insurances
                                            .Where(i => i.EnginId == e.Id)
                                            .OrderByDescending(i => i.EndDate)
                                            .FirstOrDefault()
                })
                .Where(x => x.LastInsurance != null);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                    x.Engin.Matricule.Contains(search) ||
                    x.LastInsurance!.Company.Contains(search) ||
                    x.LastInsurance!.Type.Contains(search)
                );
            }

            var list = await query.ToListAsync();

            var result = list.Select(x => new InsuranceDto
            {
                EnginId = x.Engin.Id,
                Matricule = x.Engin.Matricule,
                Id = x.LastInsurance!.Id,
                Company = x.LastInsurance!.Company,
                Type = x.LastInsurance!.Type,
                Amount = x.LastInsurance!.Amount,
                startDate = x.LastInsurance!.startDate,
                EndDate = x.LastInsurance!.EndDate,
                CreatedOn = x.LastInsurance!.CreatedOn
            })
            .OrderByDescending(x => x.CreatedOn)
            .ToList();

            return Ok(result);
        }

    }
}

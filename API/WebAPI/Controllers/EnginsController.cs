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

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Engin>>> GetAutos()
        {
            var engins = await _context.Engins
                .OrderByDescending(e => e.CreatedOn)
                .ToListAsync();
            return Ok(engins);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Engin>>> GetAll(
     [FromQuery] string? matricule = null,
     [FromQuery] string? genre = null,
     [FromQuery] string? type = null,
     [FromQuery] string? dateRange = null,
     [FromQuery] int page = 1,
     [FromQuery] int pageSize = 10)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var query = _context.Engins.AsQueryable();

            if (!string.IsNullOrEmpty(matricule))
                query = query.Where(e => e.Matricule.Contains(matricule));
            if (!string.IsNullOrEmpty(genre))
                query = query.Where(e => e.Genre.Contains(genre));
            if (!string.IsNullOrEmpty(type))
                query = query.Where(e => e.Type.Contains(type));

            if (!string.IsNullOrEmpty(dateRange))
            {
                var now = DateTime.Now;
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MaxValue;

                switch (dateRange.ToLower())
                {
                    case "week":
                        int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
                        startDate = now.AddDays(-1 * diff).Date;
                        endDate = startDate.AddDays(6).Date;
                        break;
                    case "month":
                        startDate = new DateTime(now.Year, now.Month, 1);
                        endDate = startDate.AddMonths(1).AddDays(-1);
                        break;
                    case "year":
                        startDate = new DateTime(now.Year, 1, 1);
                        endDate = new DateTime(now.Year, 12, 31);
                        break;
                }

                query = query.Where(e => e.MiseCirculationDate >= startDate && e.MiseCirculationDate <= endDate);
            }

            var totalItems = await query.CountAsync();

            var engins = await query
                .OrderByDescending(e => e.CreatedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PagedResult<Engin>
            {
                Data = engins,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };

            return Ok(result);
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
        public async Task<ActionResult<PagedResult<InsuranceDto>>> GetLastInsurancePerEngin(
            [FromQuery] string? matricule = null,
            [FromQuery] string? type = null,
            [FromQuery] string? company = null,
            [FromQuery] string? dateRange = null, // ← ajout du paramètre
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var query = _context.Engins
                .Select(e => new
                {
                    Engin = e,
                    LastInsurance = _context.Insurances
                        .Where(i => i.EnginId == e.Id)
                        .OrderByDescending(i => i.EndDate)
                        .FirstOrDefault()
                })
                .Where(x => x.LastInsurance != null)
                .AsQueryable();

            if (!string.IsNullOrEmpty(matricule))
                query = query.Where(x => x.Engin.Matricule.Contains(matricule));

            if (!string.IsNullOrEmpty(type))
                query = query.Where(x => x.LastInsurance!.Type.Contains(type));

            if (!string.IsNullOrEmpty(company))
                query = query.Where(x => x.LastInsurance!.Company.Contains(company));
            if (!string.IsNullOrEmpty(dateRange))
            {
                var now = DateTime.Now;
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MaxValue;

                switch (dateRange.ToLower())
                {
                    case "week":
                        int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
                        startDate = now.AddDays(-1 * diff).Date;
                        endDate = startDate.AddDays(6).Date;
                        break;
                    case "month":
                        startDate = new DateTime(now.Year, now.Month, 1);
                        endDate = startDate.AddMonths(1).AddDays(-1);
                        break;
                    case "year":
                        startDate = new DateTime(now.Year, 1, 1);
                        endDate = new DateTime(now.Year, 12, 31);
                        break;
                }

                query = query.Where(x =>
                    x.LastInsurance!.startDate >= startDate &&
                    x.LastInsurance!.startDate <= endDate
                );
            }

            var totalItems = await query.CountAsync();

            var list = await query
                .OrderByDescending(x => x.LastInsurance!.CreatedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var data = list.Select(x => new InsuranceDto
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
            }).ToList();

            var result = new PagedResult<InsuranceDto>
            {
                Data = data,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };

            return Ok(result);
        }

        [HttpGet("lastInsuranceNPaged")]
        public async Task<ActionResult<IEnumerable<InsuranceDto>>> GetLastInsuranceNPaged()
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
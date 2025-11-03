using PATOA.APPLICATION.DTOs;
using PATOA.APPLICATION.Interfaces;
using PATOA.CORE.Entities;
using PATOA.CORE.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PATOA.INFRA.Data;

namespace PATOA.APPLICATION.Services
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IInsuranceRepository _repo;
        private readonly ApplicationDbContext _context; 

        public InsuranceService(IInsuranceRepository repo, ApplicationDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<IEnumerable<InsuranceDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(i => new InsuranceDto
            {
                Id = i.Id,
                Company = i.Company,
                Type = i.Type,
                Amount = i.Amount,
                startDate = i.startDate,
                EndDate = i.EndDate,
                EnginId = i.EnginId,
                Matricule = i.Engin?.Matricule
            });
        }

        public async Task<InsuranceDto?> GetByIdAsync(Guid id)
        {
            var i = await _repo.GetByIdAsync(id);
            if (i == null) return null;
            return new InsuranceDto
            {
                Id = i.Id,
                Company = i.Company,
                Type = i.Type,
                Amount = i.Amount,
                startDate = i.startDate,
                EndDate = i.EndDate,
                EnginId = i.EnginId,
                Matricule = i.Engin?.Matricule
            };
        }

        public async Task AddAsync(InsuranceDto dto)
        {
            var engin = await _context.Engins.FirstOrDefaultAsync(e => e.Matricule == dto.Matricule);
            if (engin == null)
                throw new Exception("Véhicule introuvable");

            var insurance = new Insurance
            {
                Id = Guid.NewGuid(),
                Company = dto.Company,
                Type = dto.Type,
                Amount = dto.Amount,
                startDate = dto.startDate,
                EndDate = dto.EndDate,
                EnginId = engin.Id
            };

            await _repo.AddAsync(insurance);
        }


        public async Task UpdateAsync(Guid id, InsuranceDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) throw new Exception("Insurance not found.");

            existing.Company = dto.Company;
            existing.Type = dto.Type;
            existing.Amount = dto.Amount;
            existing.startDate = dto.startDate;
            existing.EndDate = dto.EndDate;
            existing.EnginId = dto.EnginId;

            await _repo.UpdateAsync(existing);
        }


        public async Task DeleteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}


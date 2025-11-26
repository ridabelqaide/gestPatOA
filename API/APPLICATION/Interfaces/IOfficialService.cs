using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PATOA.CORE.Entities;
using PATOA.APPLICATION.DTOs;
namespace PATOA.APPLICATION.Interfaces
{
	public interface IOfficialService
	{
		Task<IEnumerable<Official>> GetAllOfficialsAsync();
		Task<PagedResult<Official>> GetPagedOfficialsAsync(string? genre = null,
          string? fonction = null,string? service = null,int page = 1,int pageSize = 5);
		Task<Official> GetOfficialByIdAsync(Guid id);
		Task<Official> CreateOfficialAsync(Official official);
		Task<string> UpdateOfficialAsync(Guid id,Official official);
		Task<string> DeleteOfficialAsync(Guid id);
	}
}
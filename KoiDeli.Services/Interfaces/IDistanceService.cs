using KoiDeli.Domain.DTOs.DistanceDTOs;
using KoiDeli.Domain.DTOs.RoleDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IDistanceService
    {
        
        Task<ApiResult<List<DistanceDTO>>> GetDistancesAsync();
        Task<ApiResult<List<DistanceDTO>>> GetDistancesEnabledAsync();
        Task<ApiResult<DistanceDTO>> GetDistanceByIdAsync(int id);
        Task<ApiResult<DistanceDTO>> DeleteDistanceAsync(int id);
        Task<ApiResult<DistanceDTO>> UpdateDistanceAsync(int id, DistanceUpdateDTO updateDto);
        Task<ApiResult<DistanceDTO>> CreateDistanceAsync(DistanceCreateDTO createDTO);
        
    }
}

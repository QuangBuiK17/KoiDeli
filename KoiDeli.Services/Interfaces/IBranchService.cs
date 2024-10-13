using KoiDeli.Domain.DTOs.BranchDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IBranchService
    {
        Task<ApiResult<List<BranchDTO>>> GetBranchesAsync();
        Task<ApiResult<List<BranchDTO>>> GetBranchesEnableAsync();
        Task<ApiResult<BranchDTO>> GetBranchByIdAsync(int id);
        Task<ApiResult<List<BranchDTO>>> SearchBranchByNameAsync(string name);
        Task<ApiResult<BranchDTO>> DeleteBranchAsync(int id);
        Task<ApiResult<BranchDTO>> UpdateBranchAsync(int id, BranchUpdateDTO updateDto);
        Task<ApiResult<BranchDTO>> CreateBranchAsync(BranchCreateDTO branch);

    }
}

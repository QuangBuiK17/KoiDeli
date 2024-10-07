using KoiDeli.Domain.DTOs.AccountDTOs;
using KoiDeli.Domain.DTOs.RoleDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IRoleService
    {

        Task<ApiResult<List<RoleDTO>>> GetRolesAsync();
        Task<ApiResult<List<RoleDTO>>> GetRolesEnabledAsync();
        Task<ApiResult<RoleDTO>> GetRoleByIdAsync(int id);
        Task<ApiResult<List<RoleDTO>>> SearchRoleByNameAsync(string name);
        Task<ApiResult<RoleDTO>> DeleteRoleAsync(int id);
        Task<ApiResult<RoleDTO>> UpdateRoleAsync(int id, RoleUpdateDTO updateDto);
        Task<ApiResult<RoleDTO>> CreateRoleAsync(RoleCreateDTO role);

    }
}

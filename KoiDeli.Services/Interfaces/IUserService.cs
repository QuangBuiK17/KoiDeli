using KoiDeli.Domain.DTOs.TransactionDTOs;
using KoiDeli.Domain.DTOs.UserDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResult<List<UserDTO>>> GetAsync();
        Task<ApiResult<UserDTO>> GetByIdAsync(int id);
        Task<ApiResult<UserDTO>> DeleteAsync(int id);
        Task<ApiResult<UserDTO>> UpdatetAsync(int id, UserUpdateDTO updateDto);
        Task<ApiResult<UserDTO>> CreateAsync(UserCreateDTO creaeDTO);
    }
}

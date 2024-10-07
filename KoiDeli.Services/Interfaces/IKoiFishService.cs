using KoiDeli.Domain.DTOs.KoiFishDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IKoiFishService
    {
        Task<ApiResult<List<KoiFishDTO>>> GetKoiFishAsync();
        Task<ApiResult<KoiFishDTO>> GetKoiFishByIdAsync(int id);
        Task<ApiResult<List<KoiFishDTO>>> SearchKoiFishByNameAsync(string name);
        Task<ApiResult<KoiFishDTO>> DeleteKoiFishAsync(int id);
        Task<ApiResult<KoiFishDTO>> UpdateKoiFishAsync(int id, KoiFishUpdateDTO updateDto);
        Task<ApiResult<KoiFishDTO>> CreateKoiFishAsync(KoiFishCreateDTO koiFish);

    }
}

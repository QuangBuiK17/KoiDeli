using KoiDeli.Domain.DTOs.BoxOptionDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IBoxOptionService
    {
        Task<ApiResult<List<BoxOptionDTO>>> GetBoxOptionsAsync();
        Task<ApiResult<List<BoxOptionDTO>>> GetBoxOptionsEnableAsync();
        Task<ApiResult<BoxOptionDTO>> GetBoxOptionByIdAsync(int id);
        Task<ApiResult<List<BoxOptionDTO>>> SearchBoxOptionByNameAsync(string name);
        Task<ApiResult<BoxOptionDTO>> DeleteBoxOptionAsync(int id);
        Task<ApiResult<BoxOptionDTO>> UpdateBoxOptionAsync(int id, BoxOptionUpdateDTO updateDto);
        Task<ApiResult<BoxOptionDTO>> CreateBoxOptionAsync(BoxOptionCreateDTO boxOption);

    }
}

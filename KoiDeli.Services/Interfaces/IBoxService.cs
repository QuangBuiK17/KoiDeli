using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IBoxService
    {
        Task<ApiResult<List<BoxDTO>>> GetBoxesAsync();
        Task<ApiResult<BoxDTO>> GetBoxByIdAsync(int id);
        Task<ApiResult<List<BoxDTO>>> SearchBoxByNameAsync(string name);
        Task<ApiResult<BoxDTO>> DeleteBoxAsync(int id);
        Task<ApiResult<BoxDTO>> UpdateBoxAsync(int id, BoxUpdateDTO updateDto);
        Task<ApiResult<BoxDTO>> CreateBoxAsync(BoxCreateDTO box);


    }
}

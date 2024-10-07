using KoiDeli.Domain.DTOs.BoxWithFishDetailDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IPackingService
    {
       public Task<ApiResult<List<BoxWithFishDetailDTO>>> OptimizePackingAsync(List<KoiFish> fishList, List<Box> boxList);       
    }
}

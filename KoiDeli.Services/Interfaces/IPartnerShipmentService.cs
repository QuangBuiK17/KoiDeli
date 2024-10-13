using KoiDeli.Domain.DTOs.DistanceDTOs;
using KoiDeli.Domain.DTOs.PartnerShipmentDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IPartnerShipmentService
    {
        Task<ApiResult<List<PartnerDTO>>> GetPartnersAsync();
        Task<ApiResult<List<PartnerDTO>>> GetPartnersEnabledAsync();
        Task<ApiResult<List<PartnerDTO>>> GetPartnersByNameAsync(string name);
        Task<ApiResult<PartnerDTO>> GetPartnerByIdAsync(int id);
        Task<ApiResult<PartnerDTO>> DeletePartnerAsync(int id);
        Task<ApiResult<PartnerDTO>> UpdatePartnerAsync(int id, PartnerUpdateDTO updateDto);
        Task<ApiResult<PartnerDTO>> UpdatePartnerCompleteAsync(int id, PartnerUpdateCompleteDTO updateDto);
        Task<ApiResult<PartnerDTO>> CreatePartnerAsync(PartnerCreateDTO createDTO);
    }
}

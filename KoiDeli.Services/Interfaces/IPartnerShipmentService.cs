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
        Task<ApiResult<List<PartnerShipmentDTO>>> GetPartnerShipmentsAsync();
        Task<ApiResult<PartnerShipmentDTO>> GetPartnerShipmentByIdAsync(int id);
        Task<ApiResult<List<PartnerShipmentDTO>>> SearchPartnerShipmentByNameAsync(string name);
        Task<ApiResult<PartnerShipmentDTO>> DeletePartnerShipmentAsync(int id);
        Task<ApiResult<PartnerShipmentDTO>> UpdatePartnerShipmentAsync(int id, PartnerShipmentUpdateDTO updateDto);
        Task<ApiResult<PartnerShipmentDTO>> CreatePartnerShipmentAsync(PartnerShipmentCreateDTO partnerShipment);

    }
}

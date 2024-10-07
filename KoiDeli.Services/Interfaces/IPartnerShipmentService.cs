<<<<<<< Updated upstream
﻿using KoiDeli.Domain.DTOs.PartnerShipmentDTOs;
=======
﻿using KoiDeli.Domain.DTOs.DistanceDTOs;
using KoiDeli.Domain.DTOs.PartnerShipmentDTOs;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        Task<ApiResult<List<PartnerShipmentDTO>>> GetPartnerShipmentsAsync();
        Task<ApiResult<PartnerShipmentDTO>> GetPartnerShipmentByIdAsync(int id);
        Task<ApiResult<List<PartnerShipmentDTO>>> SearchPartnerShipmentByNameAsync(string name);
        Task<ApiResult<PartnerShipmentDTO>> DeletePartnerShipmentAsync(int id);
        Task<ApiResult<PartnerShipmentDTO>> UpdatePartnerShipmentAsync(int id, PartnerShipmentUpdateDTO updateDto);
        Task<ApiResult<PartnerShipmentDTO>> CreatePartnerShipmentAsync(PartnerShipmentCreateDTO partnerShipment);

=======
        Task<ApiResult<List<PartnerDTO>>> GetPartnersAsync();
        Task<ApiResult<List<PartnerDTO>>> GetPartnersEnabledAsync();
        Task<ApiResult<PartnerDTO>> GetPartnerByIdAsync(int id);
        Task<ApiResult<PartnerDTO>> DeletePartnerAsync(int id);
        Task<ApiResult<PartnerDTO>> UpdatePartnerAsync(int id, PartnerUpdateDTO updateDto);
        Task<ApiResult<PartnerDTO>> UpdatePartnerCompleteAsync(int id, PartnerUpdateCompleteDTO updateDto);
        Task<ApiResult<PartnerDTO>> CreatePartnerAsync(PartnerCreateDTO createDTO);
>>>>>>> Stashed changes
    }
}

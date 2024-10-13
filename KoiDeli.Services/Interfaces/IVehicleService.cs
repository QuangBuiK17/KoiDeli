using KoiDeli.Domain.DTOs.VehicleDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<ApiResult<List<VehicleDTO>>> GetVehiclesAsync();
        Task<ApiResult<List<VehicleDTO>>> GetVehiclesEnableAsync();
        Task<ApiResult<VehicleDTO>> GetVehicleByIdAsync(int id);
        Task<ApiResult<List<VehicleDTO>>> SearchVehicleByNameAsync(string name);
        Task<ApiResult<VehicleDTO>> DeleteVehicleAsync(int id);
        Task<ApiResult<VehicleDTO>> UpdateVehicleAsync(int id, VehicleUpdateDTO updateDto);
        Task<ApiResult<VehicleDTO>> CreateVehicleAsync(VehicleCreateDTO vehicle);

    }
}

using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Domain.DTOs.VehicleDTOs;
using KoiDeli.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class VehicleController : BaseController
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllVehicles()
        {
            var result = await _vehicleService.GetVehiclesAsync();
            return Ok(result);
        }


        [HttpGet("name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchVehiclesByName(string name)
        {
            var result = await _vehicleService.SearchVehicleByNameAsync(name);
            return Ok(result);
        }

        //[Authorize (Roles = "Manager")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateVehicle([FromBody] VehicleCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var c = await _vehicleService.CreateVehicleAsync(createDto);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }

        // [Authorize(Roles = "Manager")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] VehicleUpdateDTO updateDto)
        {
            var c = await _vehicleService.UpdateVehicleAsync(id, updateDto);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }

        //  [Authorize(Roles = "Manager, Customer")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var c = await _vehicleService.DeleteVehicleAsync(id);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }
    }
}

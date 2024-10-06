using KoiDeli.Domain.DTOs.VehicleDTOs;
using KoiDeli.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
                return NotFound();
            return Ok(vehicle);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleDTO vehicleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _vehicleService.AddVehicleAsync(vehicleDto);
            return CreatedAtAction(nameof(Get), new { id = vehicleDto.Id }, vehicleDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleDTO vehicleDto)
        {
            if (!ModelState.IsValid || id != vehicleDto.Id)
                return BadRequest();

            await _vehicleService.UpdateVehicleAsync(vehicleDto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _vehicleService.DeleteVehicleAsync(id);
            return NoContent();
        }
    }
}

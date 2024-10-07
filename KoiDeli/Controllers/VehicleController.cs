<<<<<<< Updated upstream
﻿using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Domain.DTOs.VehicleDTOs;
using KoiDeli.Services.Interfaces;
=======
﻿using KoiDeli.Domain.DTOs.VehicleDTOs;
using KoiDeli.Services.Interfaces;
using Microsoft.AspNetCore.Http;
>>>>>>> Stashed changes
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
<<<<<<< Updated upstream
    public class VehicleController : BaseController
=======
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
>>>>>>> Stashed changes
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
<<<<<<< Updated upstream
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllVehicles()
        {
            var result = await _vehicleService.GetVehiclesAsync();
            return Ok(result);
        }


        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchVehiclesByName(string name)
        {
            var result = await _vehicleService.SearchVehicleByNameAsync(name);
            return Ok(result);
        }

        //[Authorize (Roles = "Manager")]
        [HttpPost("{new}")]
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
        [HttpPut("{id:int}")]
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
        [HttpDelete("{id:int}")]
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
=======
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
>>>>>>> Stashed changes
        }
    }
}

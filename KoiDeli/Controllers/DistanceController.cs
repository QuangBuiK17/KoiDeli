using KoiDeli.Domain.DTOs.DistanceDTOs;
using KoiDeli.Domain.DTOs.RoleDTOs;
using KoiDeli.Services.Interfaces;
using KoiDeli.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class DistanceController : BaseController
    {
        private readonly IDistanceService _distanceService;
        public DistanceController(IDistanceService distanceService)
        {
            _distanceService = distanceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDistance([FromBody] DistanceCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var d = await _distanceService.CreateDistanceAsync(createDto);
            if (!d.Success)
            {
                return BadRequest();
            }
            return Ok(d);
        }
        [HttpGet]
        public async Task<IActionResult> ViewAllDistances()
        {
            var result = await _distanceService.GetDistancesAsync();
            return Ok(result);
        }
        [HttpGet("enable")]
        public async Task<IActionResult> ViewAllDistancesEnabled()
        {
            var result = await _distanceService.GetDistancesEnabledAsync();
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ViewDistanceById(int id)
        {
            var result = await _distanceService.GetDistanceByIdAsync(id);
            return Ok(result);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDistance(int id, [FromBody] DistanceUpdateDTO updateDto)
        {
            var d = await _distanceService.UpdateDistanceAsync(id, updateDto);
            if (!d.Success)
            {
                return BadRequest(d);
            }
            return Ok(d);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDistance(int id)
        {
            var d = await _distanceService.DeleteDistanceAsync(id);
            if (!d.Success)
            {
                return BadRequest(d);
            }
            return Ok(d);
        }
    }
}

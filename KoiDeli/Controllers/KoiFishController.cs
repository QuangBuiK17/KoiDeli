using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Domain.DTOs.KoiFishDTOs;
using KoiDeli.Services.Interfaces;
using KoiDeli.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class KoiFishController: BaseController
    {
        private readonly IKoiFishService _koiFishService;
        public KoiFishController(IKoiFishService koiFishService)
        {
            _koiFishService = koiFishService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllKoiFishes()
        {
            var result = await _koiFishService.GetKoiFishAsync();
            return Ok(result);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchKoiFishesByName(string name)
        {
            var result = await _koiFishService.SearchKoiFishByNameAsync(name);
            return Ok(result);
        }

        //[Authorize (Roles = "Manager")]
        [HttpPost("{new}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateKoiFish([FromBody] KoiFishCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var c = await _koiFishService.CreateKoiFishAsync(createDto);
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
        public async Task<IActionResult> UpdateKoiFish(int id, [FromBody] KoiFishUpdateDTO updateDto)
        {
            var c = await _koiFishService.UpdateKoiFishAsync(id, updateDto);
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
        public async Task<IActionResult> DeleteKoiFish(int id)
        {
            var c = await _koiFishService.DeleteKoiFishAsync(id);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }
    }
}


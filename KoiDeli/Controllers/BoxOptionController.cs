using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Domain.DTOs.BoxOptionDTOs;
using KoiDeli.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class BoxOptionController : BaseController
    {
        private readonly IBoxOptionService _boxOptionService;
        public BoxOptionController(IBoxOptionService boxOptionService)
        {
            _boxOptionService = boxOptionService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllBoxOptions()
        {
            var result = await _boxOptionService.GetBoxOptionsAsync();
            return Ok(result);
        }

        [HttpGet("enable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllBoxOptionsEnable()
        {
            var result = await _boxOptionService.GetBoxOptionsEnableAsync();
            return Ok(result);
        }

        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchBoxOptionByID(int id)
        {
            var result = await _boxOptionService.GetBoxOptionByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchBoxOptionByName(string name)
        {
            var result = await _boxOptionService.SearchBoxOptionByNameAsync(name);
            return Ok(result);
        }

        //[Authorize (Roles = "Manager")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateBoxOption([FromBody] BoxOptionCreateRequest createRequest)
        {
            if (createRequest == null || createRequest.Boxes == null || !createRequest.Boxes.Any())
            {
                return BadRequest("Invalid request.");
            }

            var result = await _boxOptionService.CreateBoxOptionAsync(createRequest);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        // [Authorize(Roles = "Manager")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBoxOption(int id, [FromBody] BoxOptionUpdateDTO updateDto)
        {
            var c = await _boxOptionService.UpdateBoxOptionAsync(id, updateDto);
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
        public async Task<IActionResult> DeleteBoxOption(int id)
        {
            var c = await _boxOptionService.DeleteBoxOptionAsync(id);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }
    }
}

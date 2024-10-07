using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Domain.DTOs.PartnerShipmentDTOs;
using KoiDeli.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class PartnerShipmentController : BaseController
    {
        private readonly IPartnerShipmentService _partnerShipmentService;
        public PartnerShipmentController(IPartnerShipmentService partnerShipmentService)
        {
            _partnerShipmentService = partnerShipmentService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllPartnerShipments()
        {
            var result = await _partnerShipmentService.GetPartnerShipmentsAsync();
            return Ok(result);
        }


        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchPartnerShipmentByName(string name)
        {
            var result = await _partnerShipmentService.SearchPartnerShipmentByNameAsync(name);
            return Ok(result);
        }

        //[Authorize (Roles = "Manager")]
        [HttpPost("{new}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreatePartnerShipmentBox([FromBody] PartnerShipmentCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var c = await _partnerShipmentService.CreatePartnerShipmentAsync(createDto);
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
        public async Task<IActionResult> UpdatePartnerShipment(int id, [FromBody] PartnerShipmentUpdateDTO updateDto)
        {
            var c = await _partnerShipmentService.UpdatePartnerShipmentAsync(id, updateDto);
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
        public async Task<IActionResult> DeletePartnerShipment(int id)
        {
            var c = await _partnerShipmentService.DeletePartnerShipmentAsync(id);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }
    }
}

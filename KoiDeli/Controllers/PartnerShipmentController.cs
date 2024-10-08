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
        public async Task<IActionResult> ViewAllPartnerShipment()
        {
            var result = await _partnerShipmentService.GetPartnersAsync();
            return Ok(result);
        }


        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchPartnerShipmentsByid(int id)
        {
            var result = await _partnerShipmentService.GetPartnerByIdAsync(id);
            return Ok(result);
        }

        //[Authorize (Roles = "Manager")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateBox([FromBody] PartnerCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var c = await _partnerShipmentService.CreatePartnerAsync(createDto);
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
        public async Task<IActionResult> UpdatePartnerShipment(int id, [FromBody] PartnerUpdateDTO updateDto)
        {
            var c = await _partnerShipmentService.UpdatePartnerAsync(id, updateDto);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }

        // [Authorize(Roles = "Delivery")]
        [HttpPut("complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartnerShipmentComplete(int id, [FromBody] PartnerUpdateCompleteDTO updateDto)
        {
            var c = await _partnerShipmentService.UpdatePartnerCompleteAsync(id, updateDto);
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
        public async Task<IActionResult> DeletePartnerShipment(int id)
        {
            var c = await _partnerShipmentService.DeletePartnerAsync(id);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }
    }
}

<<<<<<< Updated upstream
﻿using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Domain.DTOs.PartnerShipmentDTOs;
using KoiDeli.Services.Interfaces;
=======
﻿using KoiDeli.Domain.DTOs.DistanceDTOs;
using KoiDeli.Domain.DTOs.PartnerShipmentDTOs;
using KoiDeli.Services.Interfaces;
using KoiDeli.Services.Services;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream

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
=======
        [HttpPost]
        public async Task<IActionResult> CreatePảtner([FromBody] PartnerCreateDTO createDto)
>>>>>>> Stashed changes
        {
            if (createDto == null)
            {
                return BadRequest();
            }
<<<<<<< Updated upstream
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
=======
            var d = await _partnerShipmentService.CreatePartnerAsync(createDto);
            if (!d.Success)
            {
                return BadRequest();
            }
            return Ok(d);
        }
        [HttpGet]
        public async Task<IActionResult> ViewAllPartners()
        {
            var result = await _partnerShipmentService.GetPartnersAsync();
            return Ok(result);
        }
        [HttpGet("enable")]
        public async Task<IActionResult> ViewAllPartnersEnabled()
        {
            var result = await _partnerShipmentService.GetPartnersEnabledAsync();
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ViewPartnerById(int id)
        {
            var result = await _partnerShipmentService.GetPartnerByIdAsync(id);
            return Ok(result);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePartner(int id, [FromBody] PartnerUpdateDTO updateDto)
        {
            var d = await _partnerShipmentService.UpdatePartnerAsync(id, updateDto);
            if (!d.Success)
            {
                return BadRequest(d);
            }
            return Ok(d);
        }
        [HttpPut("complete/{id:int}")]
        public async Task<IActionResult> UpdatePartnerComplete(int id, [FromBody] PartnerUpdateCompleteDTO updateDto)
        {
            var d = await _partnerShipmentService.UpdatePartnerCompleteAsync(id, updateDto);
            if (!d.Success)
            {
                return BadRequest(d);
            }
            return Ok(d);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePartner(int id)
        {
            var d = await _partnerShipmentService.DeletePartnerAsync(id);
            if (!d.Success)
            {
                return BadRequest(d);
            }
            return Ok(d);
        }

>>>>>>> Stashed changes
    }
}

using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Domain.DTOs.TimelineDeliveryDTOs;
using KoiDeli.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class TimelineDeliveryController : BaseController
    {
        private readonly ITimelineDeliveryService _timelineDeliveryService;
        public TimelineDeliveryController(ITimelineDeliveryService timelineDeliveryService)
        {
            _timelineDeliveryService = timelineDeliveryService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllTimelineDelivery()
        {
            var result = await _timelineDeliveryService.GetTimelineDeliveriesAsync();
            return Ok(result);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchTimelineDeliveryByName(string name)
        {
            var result = await _timelineDeliveryService.SearchTimelineDeliveryByNameAsync(name);
            return Ok(result);
        }

        //[Authorize (Roles = "Manager")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateTimelineDelivery([FromBody] TimelineDeliveryCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var c = await _timelineDeliveryService.CreateTimelineDeliveryAsync(createDto);
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
        public async Task<IActionResult> UpdateTimelineDelivery(int id, [FromBody] TimelineDeliveryUpdateDTO updateDto)
        {
            var c = await _timelineDeliveryService.UpdateTimelineDeliveryAsync(id, updateDto);
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
        public async Task<IActionResult> DeleteBox(int id)
        {
            var c = await _timelineDeliveryService.DeleteTimelineDeliveryAsync(id);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }
    }
}

using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Domain.DTOs.OrderTimelineDTOs;
using KoiDeli.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class OrderTimelineController : BaseController
    {
        private readonly IOrderTimelineService _orderTimelineService;
        public OrderTimelineController(IOrderTimelineService orderTimelineService)
        {
            _orderTimelineService = orderTimelineService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllOrderTimeline()
        {
            var result = await _orderTimelineService.GetOrderTimelinesAsync();
            return Ok(result);
        }

        [HttpGet("enable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllOrderTimelineEnable()
        {
            var result = await _orderTimelineService.GetOrderTimelinesEnableAsync();
            return Ok(result);
        }

        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrderTimelineById(int id)
        {
            var result = await _orderTimelineService.GetOrderTimelineByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchOrderTimelineByName(string name)
        {
            var result = await _orderTimelineService.SearchOrderTimelineByNameAsync(name);
            return Ok(result);
        }

        //[Authorize (Roles = "Manager")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateOrderTimeline([FromBody] OrderTimelineCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var c = await _orderTimelineService.CreateOrderTimelineAsync(createDto);
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
        public async Task<IActionResult> UpdateOrderTimeline(int id, [FromBody] OrderTimelineUpdateDTO updateDto)
        {
            var c = await _orderTimelineService.UpdateOrderTimelineAsync(id, updateDto);
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
        public async Task<IActionResult> DeleteOrderTimeline(int id)
        {
            var c = await _orderTimelineService.DeleteOrderTimelineAsync(id);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }
    }
}

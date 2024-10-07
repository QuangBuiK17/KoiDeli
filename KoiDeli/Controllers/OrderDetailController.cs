using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Domain.DTOs.OrderDetailDTOs;
using KoiDeli.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class OrderDetailController : BaseController
    {
        private readonly IOrderDetailService _orderDetailService;
        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllOrderDetail()
        {
            var result = await _orderDetailService.GetOrderDetailsAsync();
            return Ok(result);
        }


        [HttpGet("name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchOrderDetailByName(string name)
        {
            var result = await _orderDetailService.SearchOrderDetailByNameAsync(name);
            return Ok(result);
        }

        //[Authorize (Roles = "Manager")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateBox([FromBody] OrderDetailCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var c = await _orderDetailService.CreateOrderDetailAsync(createDto);
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
        public async Task<IActionResult> UpdateOrderDetail(int id, [FromBody] OrderDetailUpdateDTO updateDto)
        {
            var c = await _orderDetailService.UpdateOrderDetailAsync(id, updateDto);
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
            var c = await _orderDetailService.DeleteOrderDetailAsync(id);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }
    }
}

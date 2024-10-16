using KoiDeli.Domain.DTOs.OrderTimelineDTOs;
using KoiDeli.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class DeliveryController : BaseController
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet("order-detail/{orderDetailID}")]
        public async Task<IActionResult> GetOrderDetailInfo(int orderDetailID)
        {
            var result = await _deliveryService.GetOrderDetailInfoAsync(orderDetailID);
            return Ok(result);
        }

        [HttpGet("timeline-deliveries")]
        public async Task<IActionResult> GetTimelineDeliveries(int branchID, DateTime date)
        {
            var result = await _deliveryService.GetTimelineDeliveriesAsync(branchID, date);
            return Ok(result);
        }

        [HttpPost("assign-order")]
        public async Task<IActionResult> AssignOrderToTimeline(int timelineDeliveryID, int orderDetailID)
        {
            var result = await _deliveryService.AssignOrderToTimelineAsync(timelineDeliveryID, orderDetailID);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrdertimeline(OrderTimelineCreateDTO orderTimelineDto)
        {
            var result = await _deliveryService.CreateOrderTimelineAsync(orderTimelineDto);
            return Ok(result);
        }
    }
}

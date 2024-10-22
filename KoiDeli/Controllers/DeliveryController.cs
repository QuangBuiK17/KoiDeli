using KoiDeli.Domain.DTOs.DeliveryDTOs;
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

        [HttpGet("order-detail-infor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrderDetailInfo(int orderDetailID)
        {
            var result = await _deliveryService.GetOrderDetailInfoAsync(orderDetailID);
            return Ok(result);
        }

        [HttpGet("timeline-deliveries-infor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTimelineDeliveries(int branchID, DateTime date)
        {
            var result = await _deliveryService.GetTimelineDeliveriesAsync(branchID, date);
            return Ok(result);
        }
        
        [HttpGet("view-schedule-of-ordetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewScheduleOfOrdetail(int orDetailID)
        {
            var result = await _deliveryService.ViewScheduleOfOrdetail(orDetailID);
            return Ok(result);
        }

        [HttpGet("view-all-order-detail-in-timeline")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllOrderDetailInTimeline(int timelineID)
        {
            var result = await _deliveryService.ViewAllOrderDetailInTimeline(timelineID);
            return Ok(result);
        }
        /*
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateOrdertimeline(OrderTimelineCreateDTO orderTimelineDto)
        {
            var result = await _deliveryService.CreateOrderTimelineAsync(orderTimelineDto);
            return Ok(result);
        }
        */
        [HttpPost("multiple-timelines")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateTotalTimelines(de_CreateTotalTimelineDTO dto)
        {
            var result = await _deliveryService.CreateTotalTimelineAsync(dto);
            return Ok(result);
        }

        [HttpPost("assign-ordetail-timeline")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignOrderToTimelines(de_AssignOrderToTimelinesDTO dto)
        {
            var result = await _deliveryService.AssignOrderToTimelineAsync(dto);
            return Ok(result);
        }
        
        [HttpPut("update-timeline-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTimelineStatus(int timelineID)
        {
            var result = await _deliveryService.UpdateTimelineStatusAsync(timelineID);
            return Ok(result);
        }



    }
}

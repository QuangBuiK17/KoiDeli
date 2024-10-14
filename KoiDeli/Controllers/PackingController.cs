using KoiDeli.Domain.DTOs.BoxWithFishDetailDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class PackingController : BaseController
    {
        private readonly IPackingService _packingService;

        public PackingController(IPackingService packingService)
        {
            _packingService = packingService;
        }

        [HttpPost("optimize")]
        public async Task<IActionResult> OptimizePacking([FromBody] PackingRequestDTO request)
        {
            // Kiểm tra dữ liệu đầu vào trước khi xử lý
            if (request.FishList == null || !request.FishList.Any() || request.BoxList == null || !request.BoxList.Any())
            {
                return BadRequest(new { message = "FishList or BoxList cannot be null or empty." });
            }

            var result = await _packingService.OptimizePackingAsync(request.FishList, request.BoxList);

            // Kiểm tra nếu xảy ra lỗi trong quá trình xử lý
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessages ?? new List<string> { "An unknown error occurred." } });
            }

            // Tính tổng giá của tất cả các hộp đã sử dụng
            var totalPrice = result.Data.Sum(box => box.TotalPrice);

            // Trả về thông tin chi tiết về các hộp và tổng giá
            return Ok(new
            {
                Boxes = result.Data.Select(b => new
                {
                    BoxName = b.Box.Name,
                    BoxId = b.Box.Id,
                    MaxVolume = b.Box.MaxVolume,
                    RemainingVolume = b.Box.RemainingVolume,

                    // Số lần tái sử dụng hộp
                    UsageCount = b.UsageCount,

                    // Tính tổng số lượng cá trong hộp
                    TotalFish = b.Fishes.Sum(f => f.Quantity),

                    // Tính tổng thể tích của cá trong hộp
                    TotalVolume = b.TotalVolume,

                    Price = b.BoxPrice,

                    Fishes = b.Fishes.Select(f => new
                    {
                        FishId = f.Id,
                        FishSize = f.Size,
                        FishVolume = f.Volume,
                        FishDescription = f.Description,

                        // Hiển thị số lượng cá cho từng loại
                        Quantity = f.Quantity
                    })
                }),

                // Tổng giá cho tất cả các hộp
                TotalPrice = totalPrice
            });
        }
    }


}

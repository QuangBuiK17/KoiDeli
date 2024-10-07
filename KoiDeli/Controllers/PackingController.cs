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
            var result = await _packingService.OptimizePackingAsync(request.FishList, request.BoxList);

            // Kiểm tra xem kết quả có thành công hay không
            if (!result.Success)
            {
                // Trả về lỗi nếu không thành công
                return BadRequest(new { message = result.Message, errors = result.ErrorMessages });
            }

            // Trả về dữ liệu từ result.Data
            return Ok(result.Data.Select(b => new
            {
                BoxType = b.Box.Name,
                BoxId = b.Box.Id,
                MaxVolume = b.Box.MaxVolume,
                RemainingVolume = b.Box.RemainingVolume,
                TotalFish = b.TotalFish,
                Fishes = b.Fishes.Select(f => new
                {
                    FishId = f.Id,
                    FishSize = f.Size,
                    FishVolume = f.Volume,
                    FishDescription = f.Description
                })
            }));
        }

    }
}

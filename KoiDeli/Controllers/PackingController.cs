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

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessages });
            }

            // Calculate the total price of all used boxes
            var totalPrice = result.Data.Sum(box => box.TotalPrice);

            // Return box details along with total price
            return Ok(new
            {
                Boxes = result.Data.Select(b => new
                {
                    BoxName = b.Box.Name,
                    BoxId = b.Box.Id,
                    MaxVolume = b.Box.MaxVolume,
                    RemainingVolume = b.Box.RemainingVolume,

                    // TotalFish bằng tổng quantity của từng loại cá
                    TotalFish = b.Fishes.Sum(f => f.Quantity),

                    Price = b.BoxPrice,

                    Fishes = b.Fishes.Select(f => new
                    {
                        FishId = f.Id,
                        FishSize = f.Size,
                        FishVolume = f.Volume,
                        FishDescription = f.Description,

                        // Hiển thị Quantity của từng loại cá
                        Quantity = f.Quantity
                    })
                }),
                TotalPrice = totalPrice
            });
        }


    }
}

using KoiDeli.Domain.DTOs.DistanceDTOs;
using KoiDeli.Domain.DTOs.WalletDTOs;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Services.Interfaces;
using KoiDeli.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody] WalletCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var w = await _walletService.CreateWalletAsync(createDto);
            if (!w.Success)
            {
                return BadRequest();
            }
            return Ok(w);
        }
        [HttpGet]
        public async Task<IActionResult> ViewAllWallets()
        {
            var result = await _walletService.GetWalletAsync();
            return Ok(result);
        }
        [HttpGet("enable")]
        public async Task<IActionResult> ViewAllWalletsEnabled()
        {
            var result = await _walletService.GetWalletEnabledAsync();
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ViewWalletById(int id)
        {
            var result = await _walletService.GetWalletByIdAsync(id);
            return Ok(result);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateWallet(int id, [FromBody] WalletUpdateDTO updateDto)
        {
            var w = await _walletService.UpdateWalletAsync(id, updateDto);
            if (!w.Success)
            {
                return BadRequest(w);
            }
            return Ok(w);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            var w  = await _walletService.DeleteWalletAsync(id);
            if (!w.Success)
            {
                return BadRequest(w);
            }
            return Ok(w);
        }
    }
}

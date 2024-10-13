using KoiDeli.Domain.DTOs.TransactionDTOs;
using KoiDeli.Domain.DTOs.WalletDTOs;
using KoiDeli.Services.Interfaces;
using KoiDeli.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllTransactions()
        {
            var result = await _transactionService.GetAsync();
            return Ok(result);
        }

        [HttpGet("enable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllTranctionsEnabled()
        {
            var result = await _transactionService.GetEnabledAsync();
            return Ok(result);
        }
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewTrấnctionById(int id)
        {
            var result = await _transactionService.GetByIdAsync(id);
            return Ok(result);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var t = await _transactionService.CreateAsync(createDto);
            if (!t.Success)
            {
                return BadRequest();
            }
            return Ok(t);
        }

        /*
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateWallet(int id, [FromBody] TransactionUpdateDTO updateDto)
        {
            var t = await _transactionService.UpdatetAsync(id, updateDto);
            if (!t.Success)
            {
                return BadRequest(t);
            }
            return Ok(t);
        }
        */
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            var t = await _transactionService.DeleteAsync(id);
            if (!t.Success)
            {
                return BadRequest(t);
            }
            return Ok(t);
        }
    }
}

using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Domain.DTOs.BranchDTOs;
using KoiDeli.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class BranchController : BaseController
    {
        private readonly IBranchService _branchService;
        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllBranches()
        {
            var result = await _branchService.GetBranchesAsync();
            return Ok(result);
        }


        [HttpGet("name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchBranchByName(string name)
        {
            var result = await _branchService.SearchBranchByNameAsync(name);
            return Ok(result);
        }

        //[Authorize (Roles = "Manager")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateBranch([FromBody] BranchCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var c = await _branchService.CreateBranchAsync(createDto);
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
        public async Task<IActionResult> UpdateBranch(int id, [FromBody] BranchUpdateDTO updateDto)
        {
            var c = await _branchService.UpdateBranchAsync(id, updateDto);
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
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var c = await _branchService.DeleteBranchAsync(id);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }
    }
}

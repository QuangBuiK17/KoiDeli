using KoiDeli.Domain.DTOs.UserDTOs;
using KoiDeli.Domain.DTOs.WalletDTOs;
using KoiDeli.Services.Interfaces;
using KoiDeli.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KoiDeli.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService  _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var result = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (result == null || !int.TryParse(result, out int userId))
            {
                return BadRequest("Cannot find user ID.");
            }

            var user = await _userService.GetAccountByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }            
            return Ok(user);
           /* var user = await _userService.GetCurrentUserAsync();
            if (user.Success || user.Data != null)
            {
                return Ok(result);
            }
            return Unauthorized(result);*/

        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var w = await _userService.CreateAsync(createDto);
            if (!w.Success)
            {
                return BadRequest();
            }
            return Ok(w);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var w = await _userService.DeleteAsync(id);
            if (!w.Success)
            {
                return BadRequest(w);
            }
            return Ok(w);
        }
        [HttpGet]
        public async Task<IActionResult> ViewAllUsers()
        {
            var result = await _userService.GetAsync();
            return Ok(result);
        }
        [HttpGet("id")]
        public async Task<IActionResult> ViewUserById(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO updateDto)
        {
            var w = await _userService.UpdatetAsync(id, updateDto);
            if (!w.Success)
            {
                return BadRequest(w);
            }
            return Ok(w);
        }
    }
    
}

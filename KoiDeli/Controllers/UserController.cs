using KoiDeli.Domain.DTOs.UserDTOs;
using KoiDeli.Domain.DTOs.WalletDTOs;
using KoiDeli.Services.Interfaces;
using KoiDeli.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService  _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
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
        [HttpDelete("{id:int}")]
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
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ViewUserById(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPut("{id:int}")]
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

using KoiDeli.Domain.DTOs.AccountDTOs;
using KoiDeli.Domain.DTOs.RoleDTOs;
using KoiDeli.Services.Interfaces;
using KoiDeli.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleServixe)
        {
            _roleService = roleServixe;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllRoles()
        {
            var result = await _roleService.GetRolesAsync();
            return Ok(result);
        }


        [HttpGet("enable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllRolesEnabled()
        {
            var result = await _roleService.GetRolesEnabledAsync();
            return Ok(result);
        }

        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRoleByIdAsync(int id)
        {
            var result = await _roleService.GetRoleByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ViewAllRolesEnabled(string name)
        {
            var result = await _roleService.SearchRoleByNameAsync(name);
            return Ok(result);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var r = await _roleService.CreateRoleAsync(createDto);
            if (!r.Success)
            {
                return BadRequest();
            }
            return Ok(r);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleUpdateDTO updateDto)
        {
            var c = await _roleService.UpdateRoleAsync(id, updateDto);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var c = await _roleService.DeleteRoleAsync(id);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }

    }
}

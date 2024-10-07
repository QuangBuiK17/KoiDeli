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
        
        [HttpPost]
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
        [HttpGet]
        public async Task<IActionResult> ViewAllRoles()
        {
            var result = await _roleService.GetRolesAsync();
            return Ok(result);
        } 
        [HttpGet("enable")]
        public async Task<IActionResult> ViewAllRolesEnabled()
        {
            var result = await _roleService.GetRolesEnabledAsync();
            return Ok(result);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleUpdateDTO updateDto)
        {
            var c = await _roleService.UpdateRoleAsync(id, updateDto);
            if (!c.Success)
            {
                return BadRequest(c);
            }
            return Ok(c);
        }
        [HttpDelete("{id:int}")]
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

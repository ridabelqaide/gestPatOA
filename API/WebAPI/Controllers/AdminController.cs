using Microsoft.AspNetCore.Mvc;
using PATOA.APPLICATION.Interfaces;
using PATOA.APPLICATION.DTOs.AdminDTOs;

namespace PATOA.WebAPI.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // Users endpoints
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _adminService.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var user = await _adminService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost("users")]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var user = await _adminService.CreateUserAsync(createUserDto);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("users/{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
        {
            var user = await _adminService.UpdateUserAsync(id, updateUserDto);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpDelete("users/{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var result = await _adminService.DeleteUserAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpPatch("users/{id}/toggle-status")]
        public async Task<ActionResult<UserDto>> ToggleUserStatus(Guid id)
        {
            var user = await _adminService.ToggleUserStatusAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // Roles endpoints
        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            var roles = await _adminService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpGet("roles/{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(int id)
        {
            var role = await _adminService.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();
            return Ok(role);
        }

        [HttpPost("roles")]
        public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            var role = await _adminService.CreateRoleAsync(createRoleDto);
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }

        [HttpPut("roles/{id}")]
        public async Task<ActionResult<RoleDto>> UpdateRole(int id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            var role = await _adminService.UpdateRoleAsync(id, updateRoleDto);
            if (role == null)
                return NotFound();
            return Ok(role);
        }

        [HttpDelete("roles/{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            var result = await _adminService.DeleteRoleAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }


        [HttpGet("roles/permissions")]
        public async Task<ActionResult<IEnumerable<string>>> GetPermissions()
        {
            var permissions = await _adminService.GetPermissionsAsync();
            return Ok(permissions);
        }
    }
}

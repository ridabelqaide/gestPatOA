using PATOA.APPLICATION.DTOs.AdminDTOs;

namespace PATOA.APPLICATION.Interfaces
{
    public interface IAdminService
    {
        // User methods
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(Guid id);
        Task<UserDto?> ToggleUserStatusAsync(Guid id);

        // Role methods
        Task<IEnumerable<RoleDto>> GetRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto);
        Task<RoleDto?> UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto);
        Task<bool> DeleteRoleAsync(int id);
        Task<IEnumerable<string>> GetPermissionsAsync();
    }
}

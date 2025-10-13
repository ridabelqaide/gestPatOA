using AutoMapper;
using PATOA.APPLICATION.DTOs.AdminDTOs;
using PATOA.APPLICATION.Interfaces;
using PATOA.CORE.Entities;
using PATOA.CORE.Interfaces;

namespace PATOA.APPLICATION.Services
{
    public class AdminService : IAdminService
    {
        private readonly IGenericRepository<Account> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public AdminService(
            IGenericRepository<Account> userRepository,
            IGenericRepository<Role> roleRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        // User methods
        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users.Where(u => !u.IsDeleted));
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdGuidAsync(id);
            if (user == null || user.IsDeleted)
                return null;
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new Account(createUserDto.Username, createUserDto.Email);
            user.Id = Guid.NewGuid();
            user.RoleId = createUserDto.RoleId;
            user.IsActive = createUserDto.IsActive;
            user.CreatedOn = DateTime.UtcNow;
            user.UpdatedOn = DateTime.UtcNow;
            user.IsDeleted = false;
            
            // Définir le mot de passe
            user.SetPassword(createUserDto.Password);

            await _userRepository.AddAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdGuidAsync(id);
            if (user == null || user.IsDeleted)
                return null;

            // Mapper les propriétés simples
            if (!string.IsNullOrEmpty(updateUserDto.Username))
                user.Username = updateUserDto.Username;
            if (!string.IsNullOrEmpty(updateUserDto.Email))
                user.Email = updateUserDto.Email;
            if (updateUserDto.RoleId.HasValue)
                user.RoleId = updateUserDto.RoleId.Value;
            if (updateUserDto.IsActive.HasValue)
                user.IsActive = updateUserDto.IsActive.Value;
            if (updateUserDto.IsLocked.HasValue)
            {
                if (updateUserDto.IsLocked.Value)
                    user.Lock();
                else
                    user.Unlock();
            }

            // Mettre à jour le mot de passe si fourni
            if (!string.IsNullOrEmpty(updateUserDto.Password))
                user.SetPassword(updateUserDto.Password);

            user.UpdatedOn = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdGuidAsync(id);
            if (user == null || user.IsDeleted)
                return false;

            user.IsDeleted = true;
            user.UpdatedOn = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<UserDto?> ToggleUserStatusAsync(Guid id)
        {
            var user = await _userRepository.GetByIdGuidAsync(id);
            if (user == null || user.IsDeleted)
                return null;

            user.IsActive = !user.IsActive;
            user.UpdatedOn = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        // Role methods
        public async Task<IEnumerable<RoleDto>> GetRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                return null;
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            var role = new Role
            {
                Name = createRoleDto.Name,
                Description = createRoleDto.Description
            };

            await _roleRepository.AddAsync(role);
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto?> UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                return null;

            if (!string.IsNullOrEmpty(updateRoleDto.Name))
                role.Name = updateRoleDto.Name;
            if (updateRoleDto.Description != null)
                role.Description = updateRoleDto.Description;

            await _roleRepository.UpdateAsync(role);
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                return false;

            await _roleRepository.DeleteAsync(role);
            return true;
        }


        public async Task<IEnumerable<string>> GetPermissionsAsync()
        {
            // Retourner les permissions disponibles dans le système
            return new List<string>
            {
                "users.read", "users.create", "users.update", "users.delete",
                "roles.read", "roles.create", "roles.update", "roles.delete",
                "projects.read", "projects.create", "projects.update", "projects.delete",
                "checks.read", "checks.create", "checks.update", "checks.delete",
                "reports.read", "reports.create", "reports.update", "reports.delete"
            };
        }
    }
}

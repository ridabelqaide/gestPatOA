using AutoMapper;
 
using PATOA.APPLICATION.DTOs.AdminDTOs;
using PATOA.CORE.Entities;

namespace PATOA.APPLICATION.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
 
 

            // Admin mappings - Users
            CreateMap<Account, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));

            // Admin mappings - Roles
            CreateMap<Role, RoleDto>();
        }
    }
} 
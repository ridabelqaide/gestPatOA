using AutoMapper;
using PATOA.APPLICATION.Interfaces;
using PATOA.APPLICATION.Mappers;
using PATOA.APPLICATION.Services;
using PATOA.CORE.Interfaces;
using PATOA.INFRA.Data;
using PATOA.INFRA.Repositories;
using PATOA.INFRA.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PATOA.WebAPI.DI
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Register DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("INFRA")));
            
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMapper, Mapper>();

            // Register DbContext as base class
            services.AddScoped<DbContext>(provider => provider.GetService<ApplicationDbContext>());

            // Register Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        
            services.AddScoped<IAccountRepository, AccountRepository>();
        

            // Register Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
       
            services.AddScoped<IAdminService, AdminService>();

            return services;
        }
    }
} 
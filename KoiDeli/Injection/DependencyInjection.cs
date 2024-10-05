
using FluentValidation.AspNetCore;
using KoiDeli.Domain.Entities;
using KoiDeli.MiddleWares;
using KoiDeli.Repositories;
using KoiDeli.Repositories.Common;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Services.Mapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace KoiDeli.Injection
{
    public static class DependencyInjection
    {
        public static IServiceCollection ServicesInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // CONNECT TO DATABASE
            services.AddDbContext<KoiDeliDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            

            //sign up for middleware
            services.AddSingleton<GlobalExceptionMiddleware>();
            services.AddTransient<PerformanceTimeMiddleware>();
            services.AddScoped<UserStatusMiddleware>(); // sử dụng ClaimsIdentity nên dùng Addscoped theo request
            //others
            services.AddScoped<ICurrentTime, CurrentTime>();
            services.AddSingleton<Stopwatch>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MapperConfigProfile).Assembly);
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHealthChecks();
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddMemoryCache();

            // add repositories


            // add generic repositories


            // add signInManager

            // add services


            // add unitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}


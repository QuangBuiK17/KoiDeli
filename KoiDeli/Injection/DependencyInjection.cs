
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
            services.AddScoped<IAccountRepository,AccountRepository >();
            services.AddScoped<IKoiFishRepository, KoiFishRepository>();
            services.AddScoped<IBoxRepository, BoxRepository>();
            services.AddScoped<IBoxOptionRepository, BoxOptionRepository >();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IPartnerShipmentRepository, PartnerShipmentRepository>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

            services.AddScoped<IOrderTimelineRepository, OrderTimelineRepository>();
            services.AddScoped<ITimelineDeliveryRepository, TimelineDeliveryRepository>();


            // add generic repositories
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<IGenericRepository<Box>, GenericRepository<Box>>();
            services.AddScoped<IGenericRepository<BoxOption>, GenericRepository<BoxOption>>();
            services.AddScoped<IGenericRepository<KoiFish>, GenericRepository<KoiFish>>();
            services.AddScoped<IGenericRepository<Vehicle>, GenericRepository<Vehicle>>();
            services.AddScoped<IGenericRepository<Branch>, GenericRepository<Branch>>();
            services.AddScoped<IGenericRepository<PartnerShipment>, GenericRepository<PartnerShipment>>();

            services.AddScoped<IGenericRepository<Order>, GenericRepository<Order>>();
            services.AddScoped<IGenericRepository<OrderDetail>, GenericRepository<OrderDetail>>();

            services.AddScoped<IGenericRepository<TimelineDelivery>, GenericRepository<TimelineDelivery>>();
            services.AddScoped<IGenericRepository<OrderTimeline>, GenericRepository<OrderTimeline>>();

            // add signInManager

            // add services
            services.AddScoped<IAuthenticationService, AuthenticationService >();
            services.AddScoped<IPackingService, PackingService>();
            services.AddScoped<IBoxService, BoxService>();
            services.AddScoped<IKoiFishService, KoiFishService>();
            services.AddScoped<IPartnerShipmentService, PartnerShipmentService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IBoxOptionService, BoxOptionService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();

            services.AddScoped<IOrderTimelineService, OrderTimelineService>();
            services.AddScoped<ITimelineDeliveryService, TimelineDeliveryService>();


            // add unitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}


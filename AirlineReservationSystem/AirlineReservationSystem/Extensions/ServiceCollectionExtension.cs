using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Services;
using AirlineReservationSystem.Data;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AirlineReservationSystem.Extensions
{
    namespace Microsoft.Extensions.DependencyInjection
    {
        public static class ServiceCollectionExtension
        {
            public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            {
                services.AddScoped<IApplicatioDbRepository, ApplicatioDbRepository>();
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IAircraftService, AircraftService>();
                services.AddScoped<IFlightRouteService, FlightRouteService>();
                    

                return services;
            }

            public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services, IConfiguration config)
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));
                services.AddDatabaseDeveloperPageExceptionFilter();

                return services;
            }
        }
    }
}

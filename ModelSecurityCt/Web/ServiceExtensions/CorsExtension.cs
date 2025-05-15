using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Web.ServiceExtensions
{
    public static class CorsExtension
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)  
        { 
            var origenesPermitidos = configuration
                .GetValue<string>("OrigenesPermitidos")!
                .Split(",");

            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(origenesPermitidos)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}

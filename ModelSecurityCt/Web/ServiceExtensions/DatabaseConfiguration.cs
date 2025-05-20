using Entity.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Web.FactoryDataBase;

namespace Web.ServiceExtensions
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddCustomDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            var service = new SelectionDataBase(configuration);
            var dbFactory = service.GetFactory();

            // Registras el ApplicationDbContext directamente
            services.AddScoped(provider => (ApplicationDbContext)dbFactory.CreateDbContext());

            return services;
        }
    }
}

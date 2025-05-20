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
            var databaseProvider = configuration["DatabaseProvider"];

            switch (databaseProvider)
            {
                case "SqlServer":
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
                    break;
                case "MySql":
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseMySql(configuration.GetConnectionString("MySqlConnection"),
                                         ServerVersion.AutoDetect(configuration.GetConnectionString("MySqlConnection"))));
                    break;
                case "Postgres":
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
                    break;
                default:
                    throw new InvalidOperationException($"Proveedor de base de datos '{databaseProvider}' no soportado.");
            }

            

            return services;
        }
    }

}

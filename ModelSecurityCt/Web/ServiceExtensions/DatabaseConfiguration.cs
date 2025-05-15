using Entity.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace Web.ServiceExtensions
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddCustomDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            var dbProvider = configuration["DatabaseProvider"];

            services.AddDbContext<ApplicationDbContext>(option =>
            {
                switch (dbProvider)
                {
                    case "SqlServer":
                        option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                        break;
                    case "MySql":
                        option.UseMySql(configuration.GetConnectionString("MySqlConnection"),
                            ServerVersion.AutoDetect(configuration.GetConnectionString("MySqlConnection")));
                        break;
                    case "Postgres":
                        option.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
                        break;
                    default:
                        throw new Exception("Proveedor de base de datos no soportado");
                }
            });
            return services;
        }
    }
}

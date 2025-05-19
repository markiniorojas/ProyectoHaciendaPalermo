using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Esta clase se utiliza únicamente en tiempo de diseño por Entity Framework Core
// para construir el ApplicationDbContext cuando se ejecutan comandos como:
// dotnet ef migrations add, dotnet ef database update, etc.

namespace Entity.context
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {

            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Web");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var provider = configuration["DatabaseProvider"];
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            switch (provider)
            {
                case "SqlServer":
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                    break;
                case "MySql":
                    optionsBuilder.UseMySql(configuration.GetConnectionString("MySqlConnection"),
                        ServerVersion.AutoDetect(configuration.GetConnectionString("MySqlConnection")));
                    break;
                case "Postgres":
                case "PostgreSQL":
                    optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
                    break;
                default:
                    throw new Exception($"Proveedor de base de datos desconocido: {provider}");
            }

            return new ApplicationDbContext(optionsBuilder.Options, configuration);
        }
    }
}
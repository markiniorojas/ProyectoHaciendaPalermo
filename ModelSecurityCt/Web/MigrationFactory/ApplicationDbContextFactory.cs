using Entity.context;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Web.MigrationFactory
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // 1. Configurar IConfiguration para cargar appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Inicia la búsqueda desde el directorio actual
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Obtener la cadena de conexión
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            // Asegúrate de que "DefaultConnection" coincida con el nombre de tu cadena de conexión en appsettings.json

            // 2. Configurar DbContextOptionsBuilder
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Seleccionar el proveedor de base de datos.
            // Aquí debes usar el proveedor correcto (MySQL, SQL Server, PostgreSQL, etc.)
            // En tu caso, es MySQL:
            optionsBuilder.UseSqlServer(connectionString);
            // Si usas SQL Server:
            // optionsBuilder.UseSqlServer(connectionString);
            // Si usas PostgreSQL:
            // optionsBuilder.UseNpgsql(connectionString);


            // 3. Retornar una nueva instancia de ApplicationDbContext
            return new ApplicationDbContext(optionsBuilder.Options, configuration);
        }
    }
}

using Web.ImplementacionBaseDatos;
using Web.InterfaceDb;

namespace Web.FactoryDataBase
{
    public class SelectionDataBase
    {
        private readonly IConfiguration _configuration;

        public SelectionDataBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public FactoryInterface GetFactory()
        {
            var provider = _configuration["DatabaseProvider"];
            var connectionString = _configuration.GetConnectionString(provider);

            return provider switch
            {
                "SqlServer" => new SqlServer(connectionString, _configuration),
                "Postgres" => new Postgres(connectionString, _configuration),
                "MySql" => new MySql(connectionString, _configuration),
                _ => throw new InvalidOperationException("Proveedor de base de datos no soportado")
            };
        }
    }
}

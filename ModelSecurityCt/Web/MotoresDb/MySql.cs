using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entity.context;
using Web.InterfaceDb;

namespace Web.ImplementacionBaseDatos
{
    public class MySql : FactoryInterface
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public MySql(string connectionString, IConfiguration configuration)
        {
            _connectionString = connectionString;
            _configuration = configuration;
        }

        public ApplicationDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
            return new ApplicationDbContext(optionsBuilder.Options, _configuration);
        }
    }
}
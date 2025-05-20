using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.InterfaceDb;
using Entity.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Web.ImplementacionBaseDatos;

public class SqlServer : FactoryInterface
{
    private readonly string _connectionString;
    private readonly IConfiguration _configuration;

    public SqlServer(string connectionString, IConfiguration configuration)
    {
        _connectionString = connectionString;
        _configuration = configuration;
    }

    public ApplicationDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(_connectionString);
        return new ApplicationDbContext(optionsBuilder.Options, _configuration);
    }
}
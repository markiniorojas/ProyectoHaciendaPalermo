using Entity.context;
using Microsoft.EntityFrameworkCore;

namespace Web.InterfaceDb
{
    public interface FactoryInterface
    {
        ApplicationDbContext CreateDbContext();
    }
}

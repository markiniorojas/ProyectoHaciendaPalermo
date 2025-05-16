using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DataInit
{
    public static class PermissionInit
    {
        public static void SeedPermission(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().HasData(
                new Permission
                {
                    Id = 1,
                    Name = "Crear",
                    Description = "Permite crear registros",
                    IsDeleted = false
                },
                new Permission
                {
                    Id = 2,
                    Name = "Editar",
                    Description = "Permite editar registros",
                    IsDeleted = false
                },
                new Permission
                {
                    Id = 3,
                    Name = "Eliminar",
                    Description = "Permite eliminar registros",
                    IsDeleted = false
                },
                new Permission
                {
                    Id = 4,
                    Name = "Ver",
                    Description = "Permite ver registros",
                    IsDeleted = false
                }
            );
        }
    }
}

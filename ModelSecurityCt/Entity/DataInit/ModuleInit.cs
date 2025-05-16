using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model;

namespace Entity.DataInit
{
    public static class ModuleInit
    {
        public static void SeedModule(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Module>().HasData(
                new Module
                {
                    Id = 1,
                    Name = "Administración",
                    Description = "Módulo de administración general",
                    IsDeleted = false
                },
                new Module
                {
                    Id = 2,
                    Name = "Gestión de usuarios",
                    Description = "Control y mantenimiento de usuarios",
                    IsDeleted = false
                },
                new Module
                {
                    Id = 3,
                    Name = "Reportes",
                    Description = "Visualización y exportación de reportes",
                    IsDeleted = false
                }
            );
        }
    }
}

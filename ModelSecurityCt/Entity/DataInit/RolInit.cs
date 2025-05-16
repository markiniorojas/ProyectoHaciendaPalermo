using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DataInit
{
    public static class RolInit
    {
        public static void SeedRol(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>().HasData(
                new Rol
                {
                    Id = 1,
                    Name = "Administrador",
                    Description = "Rol con todos los permisos del sistema",
                    IsDeleted = false
                },
                new Rol
                {
                    Id = 2,
                    Name = "Usuario",
                    Description = "Rol con permisos limitados",
                    IsDeleted = false
                }
            );
        }
    }
}

using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DataInit
{
    public static class FormInit
    {
        public static void SeedForm(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Form>().HasData(
                new Form
                {
                    Id = 1,
                    Name = "Formulario Principal",
                    Description = "Formulario principal del sistema",
                    Url = "/dashboard",
                    IsDeleted = false
                },
                new Form
                {
                    Id = 2,
                    Name = "Gestión de Usuarios",
                    Description = "Formulario para administración de usuarios",
                    Url = "/usuarios",
                    IsDeleted = false
                },
                new Form
                {
                    Id = 3,
                    Name = "Reportes",
                    Description = "Formulario para visualizar reportes",
                    Url = "/reportes",
                    IsDeleted = false
                }
            );
        }
    }
}

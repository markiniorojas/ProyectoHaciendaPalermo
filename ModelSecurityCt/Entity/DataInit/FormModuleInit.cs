using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DataInit
{
    public static class FormModuleInit
    {
        public static void SeedFormModule(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormModule>().HasData(
                new FormModule
                {
                    Id = 1,
                    FormId = 1,
                    ModuleId = 1,
                    IsDeleted = false
                },
                new FormModule
                {
                    Id = 2,
                    FormId = 2,
                    ModuleId = 1,
                    IsDeleted = false
                },
                new FormModule
                {
                    Id = 3,
                    FormId = 2,
                    ModuleId = 2,
                    IsDeleted = false
                }
            );
        }
    }
}

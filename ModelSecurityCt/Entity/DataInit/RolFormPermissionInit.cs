using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DataInit
{
    public static class RolFormPermissionInit
    {
        public static void SeedRolFormPermission(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolFormPermission>().HasData(
                new RolFormPermission
                {
                    Id = 1,
                    RolId = 1,
                    FormId = 1,
                    PermissionId = 1,
                    IsDeleted = false
                },
                new RolFormPermission
                {
                    Id = 2,
                    RolId = 1,
                    FormId = 1,
                    PermissionId = 2,
                    IsDeleted = false
                },
                new RolFormPermission
                {
                    Id = 3,
                    RolId = 2,
                    FormId = 2,
                    PermissionId = 4,
                    IsDeleted = false
                }
            );
        }
    }
}

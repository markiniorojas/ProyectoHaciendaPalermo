using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DataInit
{
    public static class RolUserInit
    {
        public static void SeedRolUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolUser>().HasData(
                new RolUser
                {
                    Id = 1,
                    RolId = 1,
                    UserId = 1,
                    Email = "admin@example.com",
                    IsDeleted = false
                },
                new RolUser
                {
                    Id = 2,
                    RolId = 2,
                    UserId = 2,
                    Email = "user@example.com",
                    IsDeleted = false
                }
            );
        }
    }
}

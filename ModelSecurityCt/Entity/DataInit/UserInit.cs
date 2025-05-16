using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DataInit
{
    public static class UserInit
    {
        public static void SeedUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "juan.perez@example.com",
                    Password = "1234", 
                    Active = true,
                    PersonId = 1,
                    IsDeleted = false
                },
                new User
                {
                    Id = 2,
                    Email = "maria.gomez@example.com",
                    Password = "abcd", 
                    Active = true,
                    PersonId = 2,
                    IsDeleted = false
                }
            );
        }
    }
}

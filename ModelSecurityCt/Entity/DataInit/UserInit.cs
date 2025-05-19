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
                    Email = "marcosrojasalvarez09172007@gmail.com",
                    Password = "1234", 
                    Active = true,
                    PersonId = 1,
                    IsDeleted = false
                },
                new User
                {
                    Id = 2,
                    Email = "gentilrojas@gmail.com",
                    Password = "123", 
                    Active = true,
                    PersonId = 2,
                    IsDeleted = false
                }
            );
        }
    }
}

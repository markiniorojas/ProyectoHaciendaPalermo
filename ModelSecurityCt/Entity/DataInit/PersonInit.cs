using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DataInit
{
    public static class PersonInit
    {
        public static void SeedPerson(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person
                {
                    Id = 1,
                    FirstName = "Juan",
                    LastName = "Pérez",
                    DocumentType = "CC",
                    Document = "123456789",
                    DateBorn = new DateTime(1990, 5, 15),
                    PhoneNumber = "3001234567",
                    Eps = "Nueva EPS",
                    Genero = "Masculino",
                    RelatedPerson = false,
                    IsDeleted = false
                },
                new Person
                {
                    Id = 2,
                    FirstName = "María",
                    LastName = "Gómez",
                    DocumentType = "TI",
                    Document = "987654321",
                    DateBorn = new DateTime(2000, 8, 20),
                    PhoneNumber = "3019876543",
                    Eps = "Sura",
                    Genero = "Femenino",
                    RelatedPerson = true,
                    IsDeleted = false
                }
            );
        }
    }
}

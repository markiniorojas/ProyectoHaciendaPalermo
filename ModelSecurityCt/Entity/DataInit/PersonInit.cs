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
                    FirstName = "Marcos",
                    LastName = "Rojas Alvarez",
                    DocumentType = "TI",
                    Document = "1075455",
                    DateBorn = new DateTime(1990, 5, 15),
                    PhoneNumber = "30012345",
                    Eps = "Nueva EPS",
                    Genero = "Masculino",
                    RelatedPerson = false,
                    IsDeleted = false
                },
                new Person
                {
                    Id = 2,
                    FirstName = "Gentil",
                    LastName = "Rojas Cortes",
                    DocumentType = "CC",
                    Document = "121243333",
                    DateBorn = new DateTime(1965, 3, 25),
                    PhoneNumber = "30198763",
                    Eps = "Sura",
                    Genero = "Masculino",
                    RelatedPerson = true,
                    IsDeleted = false
                }
            );
        }
    }
}

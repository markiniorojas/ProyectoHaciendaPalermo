using System;
using System.ComponentModel.DataAnnotations;

namespace Entity.Model
{
    /// <summary>
    /// Entidad que representa los datos personales asociados a un usuario
    /// </summary>
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocumentType { get; set; }
        public string Document { get; set; }
        public DateTime DateBorn { get; set; }
        public string PhoneNumber { get; set; }
        public string Eps { get; set; }
        public string Genero { get; set; }
        public bool RelatedPerson { get; set; }
        public bool IsDeleted { get; set; }

        public List<User> Users { get; set; } = new List<User>();
    }
}
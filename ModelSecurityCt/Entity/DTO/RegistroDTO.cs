using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class RegistroDTO
    {
        // Información de Person
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocumentType { get; set; }
        public string Document { get; set; }
        public DateTime DateBorn { get; set; }
        public string PhoneNumber { get; set; }
        public string Eps { get; set; }
        public string Genero { get; set; }

        // Información de User
        public string Email { get; set; }
        public string Password { get; set; }

        public List<int> RolIds { get; set; } = new List<int>();

    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    /// <summary>
    /// Entidad que representa un usuario del sistema
    /// </summary>
    public class  User : IAuditableEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; } 
        public int? PersonId { get; set; }
        public virtual Person Person { get; set; }


        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; } 

        public List<RolUser> RolUsers { get; set; } = new List<RolUser>();
    }
}
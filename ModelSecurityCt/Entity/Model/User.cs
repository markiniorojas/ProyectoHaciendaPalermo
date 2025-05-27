using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    /// <summary>
    /// Entidad que representa un usuario del sistema
    /// </summary>
    public class  User : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int? PersonId { get; set; }
        public virtual Person Person { get; set; }

        public List<RolUser> RolUsers { get; set; } = new List<RolUser>();
    }
}
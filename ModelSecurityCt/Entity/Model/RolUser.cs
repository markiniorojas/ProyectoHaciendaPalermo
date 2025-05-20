using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class RolUser : IAuditableEntity
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
        public string Email { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public Rol Rol { get; set; }    
        public User User { get; set; }
    }
}

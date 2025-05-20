using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class FormModule : IAuditableEntity
    {
        public int Id { get; set; }
        public int FormId { get; set; } 
        public int ModuleId { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public Form Form { get; set; }
        public Module Module { get; set; }
    }
}

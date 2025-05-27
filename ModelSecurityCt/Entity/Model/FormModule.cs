using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class FormModule : BaseModel
    {
        public int FormId { get; set; } 
        public int ModuleId { get; set; }

        public Form Form { get; set; }
        public Module Module { get; set; }
    }
}

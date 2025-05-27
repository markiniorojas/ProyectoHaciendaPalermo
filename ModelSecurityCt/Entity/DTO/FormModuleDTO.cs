using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class FormModuleDTO : BaseModelDTO
    {
        public int FormId { get; set; }
        public string ModuleName { get; set; }
        public string FormName { get; set; }
        public int ModuleId { get; set; }
    }
}

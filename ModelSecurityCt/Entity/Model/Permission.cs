using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Permission : BaseModel
    {
        public string Description { get; set; }


        public List<RolFormPermission> RolFormPermission { get; set; } = new List<RolFormPermission>();
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class BaseModelDTO
    {
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool Active { get; set; }
    }
}

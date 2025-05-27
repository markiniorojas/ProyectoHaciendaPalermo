using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class UserDTO : BaseModelDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
    }
}

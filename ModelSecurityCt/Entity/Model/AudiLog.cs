using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    [Table("AuditLogs", Schema = "Seguridad")]

    public class AudiLog
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string NameTable { get; set; }
        public string ActionType { get; set; }
        public DateTime TypeStamp { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string InformationType { get; set; }
    }
}

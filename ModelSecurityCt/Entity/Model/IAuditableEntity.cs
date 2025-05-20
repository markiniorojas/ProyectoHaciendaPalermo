using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; } // Nullable porque inicialmente no hay actualización
        DateTime? DeletedDate { get; set; } // Nullable para borrado lógico
        bool IsDeleted { get; set; } // Ya la tienes, la incluimos para coherencia
    }
}

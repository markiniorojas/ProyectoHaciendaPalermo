using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core;
using Entity.DTO;
using Entity.Model;

namespace Data.Interfaces
{
    public interface IRolUserRepository : IRepository<RolUser>
    {
        Task<IEnumerable<RolUserDTO>> GetAllAsync(); // Modifica el tipo de retorno aquí
        Task<RolUser?> GetByIdAsync(int id);
    }
}

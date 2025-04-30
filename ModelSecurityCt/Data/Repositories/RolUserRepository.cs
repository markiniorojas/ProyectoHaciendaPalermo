using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core;
using Data.Interfaces;
using Entity.context;
using Entity.DTO; // Asegúrate de tener este using
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public class RolUserRepository : GenericRepository<RolUser>, IRolUserRepository
    {
        private readonly ApplicationDbContext _context;
        public RolUserRepository(ApplicationDbContext context, ILogger<RolUserRepository> logger)
            : base(context, logger)
        {
            _context = context; // Asegúrate de tener acceso al contexto
        }
        public async Task<IEnumerable<RolUserDTO>> GetAllAsync()
        {
            return await _context.RolUser
                .Include(ru => ru.Rol)
                .Include(ru => ru.User)
                    .ThenInclude(u => u.Person) // Incluye la tabla Person a través de User
                .Select(ru => new RolUserDTO // Proyecta directamente a RolUserDTO
                {
                    Id = ru.Id,
                    RolId = ru.RolId,
                    RolName = ru.Rol.Name, // Asumiendo que la propiedad del nombre del rol es "Name"
                    Email = ru.User.Email,
                    UserId = ru.UserId,
                    IsDeleted = ru.IsDeleted
                })
                .ToListAsync();
        }

        public async Task<RolUser?> GetByIdAsync(int id)
        {
            return await _context.Set<RolUser>()
                .Include(ru => ru.Rol)
                .Include(ru => ru.User)
                    .ThenInclude(u => u.Person)
                .FirstOrDefaultAsync(ru => ru.Id == id);
        }
    }
}
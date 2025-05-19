using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Entity.Model;
using Entity.DTO;
using Data.Core;    
using Business.Interfaces;
using Business.Core;
using Data.Interfaces;

namespace Business.Services
{
    public class UserService : ServiceBase<UserDTO, User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository,  ILogger<UserService> logger) 
            : base(userRepository, logger)
        { 
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Consulta si el usuario es un admin o es un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> EsAdminAsync(int userId)
        {
            var user = await _userRepository.GetUserWithRolesAsync(userId);

            // Verifica si el usuario existe
            if (user == null)
                return false;

            // Asegúrate de que la relación rolUsers esté cargada
            // Si tu repositorio no carga automáticamente las relaciones, es posible que necesites
            // un método específico para cargar un usuario con sus roles

            // Verifica si el usuario tiene el rol de administrador (RolId = 1)
            return user.RolUsers.Any(ru => ru.RolId == 1 && !ru.IsDeleted);
        }

        public async Task<List<int>> GetRoleIdsByUserIdAsync(int userId)
        {
            return await _userRepository.GetRoleIdsByUserIdAsync(userId);
        }
    }
}
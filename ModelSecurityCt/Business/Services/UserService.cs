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
            var user = await _userRepository.GetByIdAsync(userId);
            // Verifica si el usuario existe
            if (user == null)
                return false;

            // Asumiendo que tienes una propiedad RoleId en tu clase User
            return user.RoleId == 1; // 1 es el ID del rol admin
        }
    }
}
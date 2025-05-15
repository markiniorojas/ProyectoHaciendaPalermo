using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core;
using Data.Interfaces;
using Entity.context;
using Entity.DTO;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    /// <summary>
    /// Repositorio concreto para la entidad User.
    /// Hereda los métodos genéricos de GenericRepository e implementa IUserRepository,
    /// permitiendo así extender o sobreescribir funcionalidades específicas para usuarios.
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {

        /// <summary>
        /// Constructor del repositorio de usuarios.
        /// Recibe el contexto de base de datos y el logger para rastreo de operaciones.
        /// </summary>
        /// <param name="context">Instancia de ApplicationDbContext para acceso a datos.</param>
        /// <param name="logger">Instancia de ILogger para registrar logs y advertencias.</param>
        public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
            : base(context, logger)
        {
        }


        /// <summary>
        /// Obtiene un usuario con sus datos de persona relacionados
        /// </summary>
        /// 
        public async Task<User> validacionUser(LoginDTO dto)
        {
            bool sucess = false;

            var user = await _context.Set<User>().FirstOrDefaultAsync(u =>
                u.Email == dto.Email &&
                u.Password == dto.Password
            );

            sucess = (user != null) ? true : throw new UnauthorizedAccessException("credenciales Incorrectas");

            return user;
        }

        ///<Summary>
        ///Metodo para el auth con google
        /// </Summary>
        /// 

        public async Task<User?> getByEmail(string email)
        {
            return await _context.user.FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}
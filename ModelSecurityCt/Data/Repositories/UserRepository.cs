using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core;
using Data.Interfaces;
using Email.Interface;
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
        private readonly ApplicationDbContext _context;
        private readonly IMensajeEmail _mensaje;
        private readonly IMensajeTelegram _mensajeTelegram;
        private readonly ILogger<UserRepository> _logger;
      
        /// <summary>
        /// Constructor del repositorio de usuarios.
        /// Recibe el contexto de base de datos y el logger para rastreo de operaciones.
        /// </summary>
        /// <param name="context">Instancia de ApplicationDbContext para acceso a datos.</param>
        /// <param name="logger">Instancia de ILogger para registrar logs y advertencias.</param>
        public UserRepository(ApplicationDbContext context, IMensajeEmail mensaje, IMensajeTelegram mensajeTelegram,
            ILogger<UserRepository> logger)
            : base(context, logger)
        {
            _context = context;
            _mensaje = mensaje;
            _mensajeTelegram = mensajeTelegram;
            _logger = logger;
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
        /// <summary> 
        /// Metodo para enviar un correo de notificacion
        /// </summary>
        /// 
        public async Task NotificarPorCorreo(string email)
        {
            string asunto = "Notificación importante";
            string contenido = "<h1>Hola, esto es un correo de prueba</h1>";
            await _mensaje.EnviarAsync(email, asunto, contenido);
        }
        /// <summary>
        /// Metodo para enviar un mensaje por telegram
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public async Task NotificarPorTelegram(string texto)
        {
            await _mensajeTelegram.EnviarTelegram(texto);
        }

        public async Task<User> GetUserWithRolesAsync(int userId)
        {
            return await _context.user
                .Include(u => u.RolUsers)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }


        public async Task<List<int>> GetRoleIdsByUserIdAsync(int userId)
        {
            var roleIds = await _context.RolUser
                                        .Where(ru => ru.UserId == userId && !ru.IsDeleted)
                                        .Select(ru => ru.RolId)
                                        .ToListAsync();

            return roleIds;
        }



    }
}
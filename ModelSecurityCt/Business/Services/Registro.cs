//using Entity.context;
//using Entity.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Entity.DTO;
//using Web.Custom.Utilidades;

//namespace Business.Services
//{
//    public class AuthService
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly Utilidades _utilidades;

//        public AuthService(ApplicationDbContext context, Utilidades utilidades)
//        {
//            _context = context;
//            _utilidades = utilidades;
//        }

//        public async Task<bool> RegistrarseAsync(UserRegisterDTO registroDTO)
//        {
//            using var transaction = _context.Database.BeginTransaction();
//            try
//            {
//                var nuevaPersona = new Person
//                {
//                    FirstName = registroDTO.FirstName,
//                    LastName = registroDTO.LastName,
//                    // ... otras propiedades de Person
//                };

//                _context.Person.Add(nuevaPersona);
//                await _context.SaveChangesAsync();

//                var nuevoUsuario = new User
//                {
//                    PersonId = nuevaPersona.Id,
//                    Email = registroDTO.Email,
//                    Password = _utilidades.encriptarSHA256(registroDTO.Password),
//                    // ... otras propiedades de User
//                };

//                _context.User.Add(nuevoUsuario);
//                await _context.SaveChangesAsync();

//                await transaction.CommitAsync();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error al registrar usuario: {ex.Message}");
//                await transaction.RollbackAsync();
//                return false;
//            }
//        }

//        // Otros métodos de autenticación (LoginAsync, etc.) también usarían _context directamente
//    }
//}

using Entity.context;
using Entity.DTO;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories;

    public class RegistroRepository
    {
        private readonly ILogger<RegistroRepository> _logger;
        private readonly ApplicationDbContext _context;

        public RegistroRepository(ILogger<RegistroRepository> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _context.Set<User>()
                    .FirstOrDefaultAsync(u =>
                    u.Email == email &&
                    u.Password == password &&
                    u.Active);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar al usuario con su email {email}", email);
                throw;
            }
        }

        public async Task<bool> Registrar(RegistroDTO registroDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var person = new Person
                {
                    FirstName = registroDTO.FirstName,
                    LastName = registroDTO.LastName,
                    DocumentType = registroDTO.DocumentType,
                    Document = registroDTO.Document,
                    DateBorn = registroDTO.DateBorn,
                    PhoneNumber = registroDTO.PhoneNumber,
                    Eps = registroDTO.Eps,
                    Genero = registroDTO.Genero,

                };
                await _context.Person.AddAsync(person);
                await _context.SaveChangesAsync();

                var user = new User
                {
                    Email = registroDTO.Email,
                    Password = registroDTO.Password,
                    PersonId = person.Id,
                    Active = true,
                    IsDeleted = false
                };

                await _context.user.AddAsync(user);
                await _context.SaveChangesAsync();

                foreach (var rolId in registroDTO.RolIds)
                {
                    var rol = await _context.Role.FindAsync(rolId);
                    if (rol != null)
                    {
                        var rolUser = new RolUser
                        {
                            UserId = user.Id,
                            RolId = rol.Id
                        };
                        await _context.RolUser.AddAsync(rolUser);
                    }
                }

            await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error al registrar el usuario");
                return false;
            }
        }

        public async Task<RolUser> getRolUserWithId(int id)
        {
            var rolUser = await _context.RolUser
                .Include(r => r.User)
                .Where(r => !r.IsDeleted && r.User.Id == id)
                .Select(r => new RolUser
                {
                    Id = r.Id,
                    RolId = r.RolId,
                    UserId = r.UserId,
                    IsDeleted = r.IsDeleted,
                })
                .FirstOrDefaultAsync();
            return rolUser;

        }
    }

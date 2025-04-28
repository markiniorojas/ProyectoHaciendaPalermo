using Business.Core;
using Data.Interfaces;
using Entity.DTO;
using Entity.Model;
using Mapster;
using Microsoft.Extensions.Logging;
using Utilities;

namespace Business.Services
{
    public class UserService : ServiceBase<UserDTO, User>
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository repository, ILogger<UserService> logger)
            : base(repository, logger) 
        {
            _userRepository = repository;
        }

        // En la clase Business.Services.UserService
        public override async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetUserWithPersonAsync(id);

            if (user == null)
                throw new EntityNotFoundException(typeof(User).Name, id);

            var userDto = user.Adapt<UserDTO>();

            _logger.LogInformation("Usuario {UserId} recuperado exitosamente", id);

            return userDto;
        }
    }
}

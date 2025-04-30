using Business.Core;
using Data.Interfaces;
using Data.Repositories;
using Entity.DTO;
using Entity.Model;
using Mapster;
using Microsoft.Extensions.Logging;
using Utilities;

namespace Business.Services
{
    public class RolUserService : ServiceBase<RolUserDTO, RolUser>
    {
        private readonly IRolUserRepository _rolUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RolUserService> _logger;

        public RolUserService(IRolUserRepository rolUserRepository, IUserRepository userRepository, ILogger<RolUserService> logger)
            : base(rolUserRepository, logger)
        {
            _rolUserRepository = rolUserRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public override async Task<IEnumerable<RolUserDTO>> GetAllAsync()
        {
            return await _rolUserRepository.GetAllAsync();
        }

        public override async Task<RolUserDTO> CreateAsync(RolUserDTO dto)
        {
            try
            {
                var entity = dto.Adapt<RolUser>();
                var createdEntity = await _rolUserRepository.AddAsync(entity);

                var user = await _userRepository.GetByIdAsync(dto.UserId);
                var createdDto = createdEntity.Adapt<RolUserDTO>();
                createdDto.Email = user?.Email;

                return createdDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear RolUser");
                throw;
            }
        }

        public override async Task<RolUserDTO> GetByIdAsync(int id)
        {
            var entity = await _rolUserRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(nameof(RolUser), id);
            }
            return entity.Adapt<RolUserDTO>();
        }

    }
}
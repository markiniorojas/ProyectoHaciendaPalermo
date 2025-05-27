using Business.Core;
using Data.Interfaces;
using Entity.DTO;
using Entity.Model;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace Business.Services
{
    public class RolUserService : ServiceBase<RolUserDTO, RolUser>
    {
        private readonly IRolUserRepository _rolUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RolUserService> _logger;

        public RolUserService(IRolUserRepository rolUserRepository,
                            IUserRepository userRepository,
                            ILogger<RolUserService> logger)
            : base(rolUserRepository, logger)
        {
            _rolUserRepository = rolUserRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public override async Task<List<RolUserDTO>> GetAllAsync()
        {
            var entities = await _rolUserRepository.GetAllAsync();
            return entities.Adapt<List<RolUserDTO>>(); // List implementa IEnumerable
        }

        public override async Task<RolUserDTO> AddAsync(RolUserDTO dto)
        {
            try
            {
                // Validación adicional
                if (dto.UserId <= 0)
                    throw new ArgumentException("UserId es requerido");

                // Lógica base
                var entity = dto.Adapt<RolUser>();
                var createdEntity = await _rolUserRepository.AddAsync(entity);

                // Lógica adicional
                var user = await _userRepository.GetByIdAsync(dto.UserId);
                var result = createdEntity.Adapt<RolUserDTO>();
                result.Email = user?.Email;

                return result;
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
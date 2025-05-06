using System.Text;
using Data.Repositories;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Mapster;
using Utilities;



namespace Business.Services;

public class RegistroService
{
    private readonly RegistroRepository _RegistroRepository;
    private readonly ILogger<RegistroService> _logger;

    public RegistroService(RegistroRepository RegistroRepository, ILogger<RegistroService> logger)
    {
        _RegistroRepository = RegistroRepository;
        _logger = logger;
    }

    async public Task<User?> Login(LoginDTO loginDTO)
    {
        try
        {
            var exists = await _RegistroRepository.LoginAsync(loginDTO.Email, loginDTO.Password);

            return exists.Adapt<User>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar al usuario con su email {email}", loginDTO.Email);
            throw;
        }
    }

    public async Task<RegistroDTO> Registro(RegistroDTO registroDTO)
    {
        try
        {
            var registroUser = await _RegistroRepository.Registrar(registroDTO);

            var result = new RegistroDTO
            {
                FirstName = registroDTO.FirstName,
                LastName = registroDTO.LastName,
                Email = registroDTO.Email,
                Password = registroDTO.Password
            };

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar al usuario con su email {email}", registroDTO.Email);
            throw;
        }
    }

    public async Task<RolUserDTO> getRolUserWithId(int id)
    {
        try
        {
            var result = await _RegistroRepository.getRolUserWithId(id);
            return result.Adapt<RolUserDTO>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el rol del usuario", ex);
        }
    }
}
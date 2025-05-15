using Business;
using Business.Services;
using Entity.DTO;
using Entity.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Utilities;
using Business.Token;
using Google.Apis.Auth;
using Data.Repositories;
using Microsoft.AspNetCore.Identity.Data;

namespace Web.Controllers;

[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
[Produces("application/json")]
public class AccesoController : ControllerBase
{
    private readonly RegistroService _registroService;
    private readonly ILogger<AccesoController> _logger;
    private readonly IConfiguration _configuration;
    private readonly generarToken _jwt;
    private readonly UserRepository _user;
    public AccesoController(RegistroService registroService, ILogger<AccesoController> logger,IConfiguration configuration, generarToken jwt, UserRepository user)
    {
        _registroService = registroService;
        _logger = logger;
        _jwt = jwt;
        _configuration = configuration;
        _user = user;
    }

    [HttpPost]

    public async Task<IActionResult> login([FromBody] LoginDTO dto)
    {
        try
        {
            var token = await _jwt.crearToken(dto);
            return StatusCode(StatusCodes.Status200OK, new { isSucces = true, token });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "validacion fallida ,error al crear el token");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("google")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleTokenDto tokenDto) 
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(tokenDto.Token, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["Google:ClientId"] }
            });

            var user = await _user.getByEmail(payload.Email);


            if (user == null)
            {
                return NotFound(new
                {
                    isSuccess = false,
                    message = "El usuario no existe"
                });
            }

            var dto = new LoginDTO
            {
                Email = user.Email,
                Password = user.Password
            };

            var token = await _jwt.crearToken(dto);

            return Ok(new { token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar al usuario");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }
}
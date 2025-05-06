using Business;
using Business.Services;
using Entity.DTO;
using Entity.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Utilities;

namespace Web.Controllers;

[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
[Produces("application/json")]
public class AccesoController : ControllerBase
{
    private readonly RegistroService _registroService;
    private readonly ILogger<AccesoController> _logger;
    private readonly Jwt _jwt;
    public AccesoController(RegistroService registroService, ILogger<AccesoController> logger, Jwt jwt)
    {
        _registroService = registroService;
        _logger = logger;
        _jwt = jwt;
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _registroService.Login(loginDTO);

            if (result == null)
            {
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
            }

            var rol = await _registroService.getRolUserWithId(result.Id);

            var token = _jwt.GenerarJwt(result, rol.RolId);

            return StatusCode(StatusCodes.Status200OK, new
            {
                isSucces = true,
                token = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al iniciar sesión");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }

    [HttpPost("Registro")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Register([FromBody] RegistroDTO registroDTO) 
    {
        try
        {
            if (registroDTO == null)
                return BadRequest(new { message = "Datos de registro inválidos. " });

            if (string.IsNullOrWhiteSpace(registroDTO.Email) || string.IsNullOrWhiteSpace(registroDTO.Password))
                return BadRequest(new { message = "Email y contraseña son obligatorios." });

            var result = await _registroService.Registro(registroDTO);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar al usuario");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }
}
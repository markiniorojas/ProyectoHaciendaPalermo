using Business;
using Business.Services;
using Entity.DTO;
using Entity.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Utilities;
using Business.Token;

namespace Web.Controllers;

[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
[Produces("application/json")]
public class AccesoController : ControllerBase
{
    private readonly RegistroService _registroService;
    private readonly ILogger<AccesoController> _logger;
    private readonly generarToken _jwt;
    public AccesoController(RegistroService registroService, ILogger<AccesoController> logger, generarToken jwt)
    {
        _registroService = registroService;
        _logger = logger;
        _jwt = jwt;
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

    [HttpPost("Registro")]
    [Authorize]
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
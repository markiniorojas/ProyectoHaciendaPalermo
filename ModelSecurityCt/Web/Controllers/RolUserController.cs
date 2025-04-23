using Business.Services;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolUserController : ControllerBase
    {
        private readonly RolUserService _RolUserBusiness;
        private readonly ILogger<RolUserController> _logger;

        public RolUserController(RolUserService rolUserBusiness, ILogger<RolUserController> _logger)
        {
            _RolUserBusiness = rolUserBusiness;
            _logger = _logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolUserDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRolUsers()
        {
            try
            {
                var RolUsers = await _RolUserBusiness.GetAllAsync();
                return Ok(RolUsers);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtner RolForm");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolUserDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolUserByIdAsync(int id)
        {
            try
            {
                var RolUser = await _RolUserBusiness.GetByIdAsync(id);
                return Ok(RolUser);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida por el RolUser con ID: {RolUserId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "RolUser no encontrado con ID: {RolUser}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtner RolUser con ID: {RolUserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(RolUserDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        [HttpPost]
        public async Task<IActionResult> CreateRolUser([FromBody] RolUserDTO rolUserDto)
        {
            if (rolUserDto == null)
                return BadRequest(new { message = "El objeto RolUser no puede ser nulo." });

            try
            {
                var newRolUser = await _RolUserBusiness.CreateAsync(rolUserDto);
                return Ok(newRolUser);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación al crear un RolUser");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error en la base de datos al crear un RolUser");
                return StatusCode(500, new { message = "Error interno del servidor al procesar la solicitud." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error desconocido al crear un RolUser");
                return StatusCode(500, new { message = "Ocurrió un error inesperado." });
            }
        }



        [HttpPut]
        [ProducesResponseType(typeof(RolUserDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRolUser([FromBody] RolUserDTO RolUserDto)
        {


            try
            {
                var updatedForm = await _RolUserBusiness.UpdateAsync(RolUserDto);
                return Ok(updatedForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar RolUser");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "rolUser no encontrado con ID {RolUserId}");
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al rolUser Form con ID {RolUserId}");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("permanent/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRol(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del rolUser debe ser mayor que cero" });
                }

                await _RolUserBusiness.DeletePermanentAsync(id);
                return Ok(new { message = "RolUser eliminado correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "RolUser no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el RolUser con ID: {RolUserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("Logico/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteRolLogical(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del rolUser debe ser mayor que cero" });
                }

                await _RolUserBusiness.DeleteLogicalAsync(id);
                return Ok(new { message = "rolUser  eliminado lógico correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "rolUser  no encontrado con ID: " + id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente  el rolUser  con ID:" + id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPatch("recuperarLogica/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> PatchLogicalAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID de el rol debe igual a cero" });
                }

                await _RolUserBusiness.PatchLogicalAsync(id);
                return Ok(new { message = "rol  restablecido lógico correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "rol no encontrado con ID: " + id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente del rol con ID:" + id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

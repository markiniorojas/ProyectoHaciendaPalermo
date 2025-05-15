using Business.Services;
using Entity.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Web.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    public class RolUserController : ControllerBase
    {
        private readonly RolUserService _rolUserBusiness;
        private readonly ILogger<RolUserController> _logger;

        public RolUserController(RolUserService rolUserBusiness, ILogger<RolUserController> _logger)
        {
            _rolUserBusiness = rolUserBusiness;
            _logger = _logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolUserDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRolUsers()
        {
            try
            {
                var RolUsers = await _rolUserBusiness.GetAllAsync();
                return Ok(RolUsers);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtner RolForm");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolUserById(int id)
        {
            try
            {
                var RolUser = await _rolUserBusiness.GetByIdAsync(id);
                return Ok(RolUser);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida por el RolUser con ID", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "RolUser no encontrado con ID: {rol}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtner RolUser con ID: {userId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(RolUserDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRolUser([FromBody] RolUserDTO rolUserDto)
        {
          
            try
            {
                var createRolUser = await _rolUserBusiness.CreateAsync(rolUserDto);
                return CreatedAtAction(nameof(GetRolUserById), new
                {
                    id = createRolUser.Id
                }, createRolUser);
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
                if(RolUserDto == null || RolUserDto.Id <= 0)
                {
                    return BadRequest(new { message = "El ID del rol debe ser mayor que cero y no nulo" });
                }
                var updatedRolUser = await _rolUserBusiness.UpdateAsync(RolUserDto);
                return Ok(updatedRolUser);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar RolUser");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "rolUser no encontrado con ID {rolId}", RolUserDto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al rolUser Form con ID {RolUserId}", RolUserDto.Id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("permanent/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRolUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del rolUser debe ser mayor que cero" });
                }

                await _rolUserBusiness.DeletePermanentAsync(id);
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

        public async Task<IActionResult> DeleteRolUserLogical(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del rolUser debe ser mayor que cero" });
                }

                await _rolUserBusiness.DeleteLogicalAsync(id);
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

                await _rolUserBusiness.PatchLogicalAsync(id);
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

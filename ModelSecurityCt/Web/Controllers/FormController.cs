using Business;
using Business.Services;
using Entity.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using Web.Exceptions;

namespace Web.Controllers
{

    [Route("api/[controller]")]
    ///[Authorize]
    [ApiController]
    [Produces("application/json")]
    public class FormController : ControllerBase
    {
        private readonly FormService _formBusiness;
        private readonly ILogger<FormController> _logger;

        /// <summary>
        /// Constructor del controlador de users
        /// </summary>
        /// <param name="FormBusiness">Capa de negocio de Forms</param>
        /// <param name="logger">Logger para registro de eventos</param>
        /// 
        public FormController(FormService _formBusiness, ILogger<FormController> _logger)
        {
            this._formBusiness = _formBusiness;
            this._logger = _logger;
        }

        /// <summary>
        /// Obtiene todos los Form del sistema
        /// </summary>
        /// <returns>Lista de Form</returns>
        /// <response code="200">Retorna la lista de Form</response>
        /// <response code="500">Error interno del servidor</response>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetAllForm()
        {
            try
            {
                var forms = await _formBusiness.GetAllAsync();
                return Ok(forms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los Forms");
                throw new RequestProcessingException("Ocurrió un error al procesar la solicitud para obtener los Forms.", ex);
            }
        }

        /// <summary>
        /// Obtiene un Form específico por su ID
        /// </summary>
        /// <param name="id">ID del Form</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retorna el Form solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor<(/response>
        /// 

        [HttpGet("{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFormById(int id)
        {
            try
            {
                var form = await _formBusiness.GetByIdAsync(id);
                return Ok(form);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, $"Validación fallida para el Form con ID: {id}");
                throw new ApiValidationException($"Validación fallida: {ex.Message}", ex);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, $"Form no encontrado con ID: {id}");
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, $"Error externo al obtener Form con ID: {id}");
                throw new ExternalIntegrationException($"Error al integrar con servicios externos al obtener el Form con ID {id}.", ex);
            }
        }



        /// <summary>
        /// Crea un nuevo form en el sistema
        /// </summary>
        /// <param name="FormDTO">Datos del form a crear</param>
        /// <returns>Module creado</returns>
        /// <response code="201">Retorna el form creado</response>
        /// <response code="400">Datos del form no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(FormDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateForm([FromBody] FormDTO FormDTO)
        {
            try
            {
                var createdForm = await _formBusiness.CreateAsync(FormDTO);
                return CreatedAtAction(nameof(GetFormById), new { id = createdForm.Id }, createdForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación al crear el Form");
                throw new ApiValidationException("Los datos del Form no son válidos.", ex);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error externo al crear el Form");
                throw new ExternalIntegrationException("Error al integrar con servicios externos al crear el Form.", ex);
            }
        }


        /// <summary>
        /// Actualiza un form existente
        /// </summary>
        /// <param name="formDTO">Datos del v a actualizar</param>
        /// <returns>form actualizado</returns>
        /// <response code="200">Retorna el form actualizado</response>
        /// <response code="400">Datos no válidos</response>
        /// <response code="404">form no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut]
        [ProducesResponseType(typeof(FormDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateModule([FromBody] FormDTO formDTO)
        {
            try
            {
                if (formDTO == null || formDTO.Id <= 0)
                {
                    throw new ApiValidationException("El ID del Form debe ser mayor que cero y no nulo.");
                }

                var updatedForm = await _formBusiness.UpdateAsync(formDTO);
                return Ok(updatedForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación al actualizar el Form");
                throw new ApiValidationException("Los datos del Form no son válidos.", ex);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, $"Form no encontrado con ID: {formDTO.Id}");
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, $"Error externo al actualizar el Form con ID: {formDTO.Id}");
                throw new ExternalIntegrationException($"Error al integrar con servicios externos al actualizar el Form con ID {formDTO.Id}.", ex);
            }
        }


        /// <summary>
        /// Elimina un form por su ID
        /// </summary>
        /// <param name="id">ID del form a eliminar</param>
        /// <returns>Mensaje de éxito</returns>
        /// <response code="200">form eliminado exitosamente</response>
        /// <response code="400">ID no válido</response>
        /// <response code="404">form no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpDelete("permanent/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteForm(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ApiValidationException("El ID del Form debe ser mayor que cero.");
                }

                await _formBusiness.DeletePermanentAsync(id);
                return Ok(new { message = "Form eliminado correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, $"Form no encontrado con ID: {id}");
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, $"Error externo al eliminar el Form con ID: {id}");
                throw new ExternalIntegrationException($"Error al integrar con servicios externos al eliminar el Form con ID {id}.", ex);
            }
        }

        [HttpPut("Logico/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteFormLogical(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ApiValidationException("El ID del Form debe ser mayor que cero.");
                }

                await _formBusiness.DeleteLogicalAsync(id);
                return Ok(new { message = "Form eliminado lógicamente correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, $"Form no encontrado con ID: {id}");
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, $"Error externo al eliminar lógicamente el Form con ID: {id}");
                throw new ExternalIntegrationException($"Error al integrar con servicios externos al eliminar lógicamente el Form con ID {id}.", ex);
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
                    throw new ApiValidationException("El ID del Form debe ser mayor que cero.");
                }

                await _formBusiness.PatchLogicalAsync(id);
                return Ok(new { message = "Form restaurado correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, $"Form no encontrado con ID: {id}");
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, $"Error externo al restaurar lógicamente el Form con ID: {id}");
                throw new ExternalIntegrationException($"Error al integrar con servicios externos al restaurar lógicamente el Form con ID {id}.", ex);
            }
        }

    }
}

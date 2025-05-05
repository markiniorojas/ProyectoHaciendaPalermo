//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Entity.Model;
//using Entity.DTO;
//using Microsoft.AspNetCore.Authorization;
//using Business.Services;
//using KeyJwt;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using System;

//namespace Web.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccesoController : ControllerBase
//    {
//        private readonly UserService _userService;
//        private readonly PersonService _personService;
//        private readonly UtilidadesService _utilidadesService;

//        public AccesoController(
//            UserService userService,
//            PersonService personService,
//            UtilidadesService utilidadesService)
//        {
//            _userService = userService;
//            _personService = personService;
//            _utilidadesService = utilidadesService;
//        }

//        [HttpPost]
//        [Route("Registrar")]
//        //[AllowAnonymous]
//        public async Task<IActionResult> Registrar([FromBody] RegistroDTO modelo)
//        {
//            try
//            {
//                // Validar si el email ya existe
//                var usuarioExistente = await _userService.GetUserByEmailAsync(modelo.Email);
//                if (usuarioExistente != null)
//                {
//                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El email ya está registrado" });
//                }

//                // Crear la persona usando PersonDTO
//                var personaDTO = new PersonDTO
//                {
//                    FirstName = modelo.FirstName,
//                    LastName = modelo.LastName,
//                    DocumentType = modelo.DocumentType,
//                    Document = modelo.Document,
//                    DateBorn = modelo.DateBorn,
//                    PhoneNumber = modelo.PhoneNumber,
//                    Eps = modelo.Eps,
//                    Genero = modelo.Genero,
//                    RelatedPerson = false
//                    // IsDeleted no se incluye en el DTO ya que es un campo interno
//                };

//                var personaCreada = await _personService.Create(personaDTO);

//                if (personaCreada == null)
//                {
//                    return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al crear la persona" });
//                }

//                // Crear el usuario usando UserDTO
//                var usuarioDTO = new UserDTO
//                {
//                    Email = modelo.Email,
//                    Password = _utilidadesService.EncriptarSHA256(modelo.Password),
//                    Active = true,
//                    RegistrationDate = DateTime.Now,
//                    PersonId = personaCreada.Id,
//                    PersonName = $"{modelo.FirstName} {modelo.LastName}"
//                    // IsDeleted no se incluye en el DTO ya que es un campo interno
//                };

//                var usuarioCreado = await _userService.Create(usuarioDTO);

//                if (usuarioCreado == null)
//                {
//                    // Si falla la creación del usuario, eliminar la persona
//                    await _personService.Delete(personaCreada.Id);
//                    return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al crear el usuario" });
//                }

//                return StatusCode(StatusCodes.Status201Created, new { mensaje = "Usuario registrado con éxito" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
//            }
//        }

//        [HttpPost]
//        [Route("Login")]
//        //[AllowAnonymous]
//        public async Task<IActionResult> Login([FromBody] LoginDTO modelo)
//        {
//            try
//            {
//                // Encriptar la contraseña para compararla
//                string passwordEncriptada = _utilidadesService.EncriptarSHA256(modelo.Password);

//                // Buscar el usuario por email y contraseña
//                var usuarioEncontrado = await _userService.GetUserByCredentialsAsync(modelo.Email, passwordEncriptada);

//                if (usuarioEncontrado == null)
//                {
//                    return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "Email o contraseña incorrectos" });
//                }

//                // Si el usuario está inactivo
//                if (!usuarioEncontrado.Active)
//                {
//                    return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "Usuario inactivo. Contacte al administrador" });
//                }

//                // Generar el token JWT
//                string token = _utilidadesService.GenerarJWT(usuarioEncontrado);

//                return StatusCode(StatusCodes.Status200OK, new
//                {
//                    token = token,
//                    usuario = new
//                    {
//                        id = usuarioEncontrado.Id,
//                        email = usuarioEncontrado.Email,
//                        nombre = usuarioEncontrado.PersonName
//                    }
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
//            }
//        }

//        [HttpGet]
//        [Route("ObtenerUsuarioActual")]
//        //[Authorize] // Requiere autenticación
//        public async Task<IActionResult> ObtenerUsuarioActual()
//        {
//            try
//            {
//                // Obtener el ID del usuario desde el token
//                var identidad = HttpContext.User.Identity as ClaimsIdentity;
//                string idUsuario = identidad?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//                if (string.IsNullOrEmpty(idUsuario))
//                {
//                    return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "Token inválido" });
//                }

//                // Buscar el usuario por ID
//                var usuario = await _userService.GetById(int.Parse(idUsuario));

//                if (usuario == null)
//                {
//                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Usuario no encontrado" });
//                }

//                // Convertimos el resultado a un objeto anónimo para devolver solo lo necesario
//                return StatusCode(StatusCodes.Status200OK, new
//                {
//                    id = usuario.Id,
//                    email = usuario.Email,
//                    nombre = usuario.PersonName
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
//            }
//        }
//    }
//}
//using Microsoft.AspNetCore.Http;

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Web.Custom;
//using Entity.Model;
//using Entity.DTO;
//using Microsoft.AspNetCore.Authorization;


//namespace Web.Controllers
//{
//    [Route("api/[controller]")]
//    [AllowAnonymous]
//    [ApiController]
//    public class AccesoController : ControllerBase
//    {
//        private readonly DbContext _context;
//        private readonly Utilidades _utilidades;

//        public AccesoController(DbContext context, Utilidades utilidades)
//        {
//            _context = context;
//            _utilidades= utilidades;
//        }

//        [HttpPost]
//        [Route("Registrarse")]
//        public async Task<IActionResult> Registrarse(UserDTO objeto)
//        {
//            var modeloUser = new User
//            {
//                PersonName = objeto.personName,
//                Email = objeto.Email,
//                Password = _utilidades.encriptarSHA256(objeto.Password)
//            };
//            await _context.User.AddAsync(modeloUser);
//            await _context.SaveChangesAsync();

//            if(modeloUser.Id > 0)
//                return StatusCode(StatusCodes.Status200OK, new { message = "Usuario creado correctamente" });
//            else
//                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al crear el usuario" });
//        }

//        [HttpPost]
//        [Route("Login")]
//        public async Task<IActionResult> Registrarse(UserDTO objeto)
//        {

//        }
//    }
//}

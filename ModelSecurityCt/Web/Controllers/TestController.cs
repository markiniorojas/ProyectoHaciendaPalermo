using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("publico")]
        [AllowAnonymous]
        public IActionResult Publico()
        {
            return Ok(new { mensaje = "Este es un endpoint público. No requiere autenticación." });
        }

        [HttpGet]
        [Route("protegido")]
        [Authorize]
        public IActionResult Protegido()
        {
            var identidad = HttpContext.User.Identity;
            return Ok(new
            {
                mensaje = "Este es un endpoint protegido. Requiere autenticación.",
                usuario = User.Identity.Name,
                estaAutenticado = identidad?.IsAuthenticated ?? false
            });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Email.Interface;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMensajeEmail _mensaje;

        public EmailController(IMensajeEmail mensaje)
        {
            _mensaje = mensaje;
        }

        [HttpPost("enviar-correo")]
        public async Task<IActionResult> EnviarCorreo([FromBody] string email)
        {
            await _mensaje.EnviarAsync(email, "buenas noches", "Un Pool o miedo?");
            return Ok("Correo enviado");
        }
    }
}

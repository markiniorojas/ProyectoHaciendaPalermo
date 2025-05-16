using Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mensajeTelegram : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public mensajeTelegram(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("telegram")]
        public async Task<IActionResult> EnviarMensajeTelegram([FromBody] string mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
                return BadRequest("El mensaje no puede estar vacío");

            await _userRepository.NotificarPorTelegram(mensaje);

            return Ok("Mensaje enviado por Telegram");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email.Interface
{
    public interface IMensajeEmail
    {
        Task EnviarAsync(string destinatario, string asunto, string contenido);
    }

    public interface IMensajeTelegram
    {
        Task EnviarTelegram(string texto);
    }
}

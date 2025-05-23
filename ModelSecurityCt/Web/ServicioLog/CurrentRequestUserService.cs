using Shared.Interface;
using System.Security.Claims;

namespace Web.ServicioLog
{
    public class CurrentRequestUserService : ICurrentRequestUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentRequestUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetCurrentUserEmail()
        {
            // Busca el claim de email. Si no lo encuentra, usa "Sistema/Desconocido"
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ??
                   "Sistema/Desconocido";
        }
    }
}

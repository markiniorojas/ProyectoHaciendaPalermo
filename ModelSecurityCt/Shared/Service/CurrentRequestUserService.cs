//using Shared.Interface;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace Shared.Service
//{
//    internal class CurrentRequestUserService : ICurrentRequestUserService
//    {
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        public CurrentRequestUserService(IHttpContextAccessor httpContextAccessor)
//        {
//            _httpContextAccessor = httpContextAccessor;
//        }

//        public string GetCurrentUserId()
//        {
//            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//        }

//        public string GetCurrentUserEmail()
//        {
//            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ??
//                   "Sistema/Desconocido";
//        }
//    }
//}

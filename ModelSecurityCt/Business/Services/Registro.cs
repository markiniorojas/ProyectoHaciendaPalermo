using System.Security.Cryptography;
using System.Text;

namespace Business.Services;

    public class UtilidadesService
    {
        private readonly string _secretKey;
        private readonly string _issuer;

        public UtilidadesService(string secretKey, string issuer)
        {
            _secretKey = secretKey;
            
        }
    }

using Entity.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Jwt
    {
        private readonly IConfiguration _configuration;
        public Jwt(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarJwt(User user, int rolId)
        {
            var userClaims = new[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("email", user.Email!),
                new Claim("role", rolId.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var JwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(JwtConfig);
        }
    }
}

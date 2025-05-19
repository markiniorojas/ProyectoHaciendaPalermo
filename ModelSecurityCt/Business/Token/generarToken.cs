using Business.Services;
using Data.Interfaces;
using Data.Repositories;
using Entity.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Token
{
    public class generarToken
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _user;

        public generarToken(IConfiguration configuration, IUserRepository user)
        {
            this._configuration = configuration;
            this._user = user;
        }

        public async Task<string> crearToken(LoginDTO dto)
        {
            var user = await _user.validacionUser(dto);
            if (user == null)
            {
                throw new UnauthorizedAccessException("credenciales incorrectas");
            }

            var roleIds = await _user.GetRoleIdsByUserIdAsync(user.Id);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)

            };

            foreach (var roleId in roleIds)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleId.ToString()));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"] ?? "apiIssuer", // Añadir un valor por defecto
                audience: _configuration["Jwt:Audience"] ?? "apiAudience", // Añadir un valor por defecto
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:Expiration"])),
                signingCredentials: credentials
            );


            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}

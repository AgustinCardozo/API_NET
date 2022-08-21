using API_Demo.Exceptions;
using API_Demo.Services.Contracts;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Demo.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private string _key;

        public JwtTokenService(string key)
        {
            _key = key;
        }

        public string Authenticate(string username, string password)
        {
            ValidationService.ValidacionDeSeguridad(password);
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new UsuarioInvalidoException("Nombre de Usuario o Contraseña incorrectos.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Email, $"{username}@mail.com"),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

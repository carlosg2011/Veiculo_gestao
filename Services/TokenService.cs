using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gestao_veiculos.Models;
using Microsoft.IdentityModel.Tokens;

namespace Gestao_veiculos.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GerarToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id_usuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Name, usuario.Nome),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expiracao = DateTime.UtcNow.AddHours(
                _configuration.GetValue<int>("Jwt:ExpirationHours"));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiracao,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

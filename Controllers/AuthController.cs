using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, ITokenService tokenService, IConfiguration configuration)
        {
            _context = context;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.Senha))
                return Unauthorized("Email ou senha inválidos.");

            var token = _tokenService.GerarToken(usuario);
            var expiracao = DateTime.UtcNow.AddHours(
                _configuration.GetValue<int>("Jwt:ExpirationHours"));

            return Ok(new TokenResponseDto
            {
                Token = token,
                Expiracao = expiracao
            });
        }
    }
}

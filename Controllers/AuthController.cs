using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Models;
using Gestao_veiculos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthController(
            AppDbContext context,
            ITokenService tokenService,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _context = context;
            _tokenService = tokenService;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.Senha))
                return Problem(detail: "Email ou senha inválidos.", statusCode: StatusCodes.Status401Unauthorized);

            var token = _tokenService.GerarToken(usuario);
            var expiracao = DateTime.UtcNow.AddHours(
                _configuration.GetValue<int>("Jwt:ExpirationHours"));

            return Ok(new TokenResponseDto
            {
                Token = token,
                Expiracao = expiracao
            });
        }

        [HttpPost("forgot-password")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario is null)
                return Problem(detail: "E-mail não encontrado.", statusCode: StatusCodes.Status404NotFound);

            // Invalida tokens anteriores não utilizados
            var tokensAntigos = await _context.PasswordResetTokens
                .Where(t => t.Id_usuario == usuario.Id_usuario && !t.Used && t.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();
            tokensAntigos.ForEach(t => t.Used = true);

            // Gera código numérico de 6 dígitos
            var bytes = new byte[4];
            RandomNumberGenerator.Fill(bytes);
            var code = (BitConverter.ToUInt32(bytes) % 900000 + 100000).ToString();

            _context.PasswordResetTokens.Add(new PasswordResetToken
            {
                Id_usuario = usuario.Id_usuario,
                Token = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            });
            await _context.SaveChangesAsync();

            await _emailService.SendPasswordResetAsync(usuario.Email, usuario.Nome, code);

            return Ok();
        }

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken(ValidateTokenDto dto)
        {
            var record = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == dto.Token && !t.Used && t.ExpiresAt > DateTime.UtcNow);

            if (record is null)
                return Problem(detail: "Código inválido ou expirado.", statusCode: StatusCodes.Status400BadRequest);

            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var resetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == dto.Token && !t.Used && t.ExpiresAt > DateTime.UtcNow);

            if (resetToken is null)
                return Problem(detail: "Código inválido ou expirado.", statusCode: StatusCodes.Status400BadRequest);

            var usuario = await _context.Usuarios.FindAsync(resetToken.Id_usuario);
            if (usuario is null)
                return Problem(detail: "Usuário não encontrado.", statusCode: StatusCodes.Status400BadRequest);

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);
            resetToken.Used = true;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

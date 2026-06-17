using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PerfilController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public PerfilController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarPerfil(UpdateProfileDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario is null)
                return Problem(detail: "Usuário não encontrado.", statusCode: StatusCodes.Status404NotFound);

            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id_usuario != userId))
                return Problem(detail: "Email já está em uso por outro usuário.", statusCode: StatusCodes.Status409Conflict);

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;
            await _context.SaveChangesAsync();

            var novoToken = _tokenService.GerarToken(usuario);

            return Ok(new
            {
                usuario = new { usuario.Id_usuario, usuario.Nome, usuario.Email, usuario.Role },
                token = novoToken
            });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> AlterarSenha(ChangePasswordDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario is null)
                return Problem(detail: "Usuário não encontrado.", statusCode: StatusCodes.Status404NotFound);

            if (!BCrypt.Net.BCrypt.Verify(dto.SenhaAtual, usuario.Senha))
                return Problem(detail: "Senha atual incorreta.", statusCode: StatusCodes.Status400BadRequest);

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

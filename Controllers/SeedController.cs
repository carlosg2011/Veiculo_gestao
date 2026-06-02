using Gestao_veiculos.Data;
using Gestao_veiculos.Enums;
using Gestao_veiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeedController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("admin")]
        public async Task<IActionResult> CriarAdmin()
        {

            if (await _context.Usuarios.AnyAsync(u => u.Role == Roles.Admin))
                return Problem(detail: "Já existe um usuário Admin cadastrado.", statusCode: StatusCodes.Status409Conflict);

            var admin = new Usuario
            {
                Nome  = "Administrador",
                Email = "admin@vehicleguard.com",
                Senha = BCrypt.Net.BCrypt.HashPassword("Admin@1234"),
                Role  = Roles.Admin
            };

            _context.Usuarios.Add(admin);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Admin criado com sucesso.",
                email    = admin.Email,
                senha    = "Admin@1234"
            });
        }
    }
}

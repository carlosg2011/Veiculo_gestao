using Gestao_veiculos.Data;
using Gestao_veiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {

        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        public IActionResult Get()
        {
            var usuario = _context.Usuarios.ToList();
            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult Post(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id_usuario = usuario.Id_usuario }, usuario);

        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id_usuario)
        {
            var usuario = _context.Usuarios.Find(id_usuario);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpDelete("{id_usuario}")]
        public IActionResult Delete(int id_usuario)
        {
            var usuario = _context.Usuarios.Find(id_usuario);
            if (usuario == null)
            {
                return NotFound();
            }
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id_usuario, Usuario updatedUsuario)
        {
            if (id_usuario != updatedUsuario.Id_usuario)
            {
                return BadRequest("O id da rota é diferente do id enviado no body.");
            }
            var usuario = _context.Usuarios.Find(id_usuario);

            if (usuario == null)
            {
                return NotFound("Usuario não encontrado");
            }
            usuario.Nome = updatedUsuario.Nome;
            usuario.Email = updatedUsuario.Email;
            usuario.Senha = updatedUsuario.Senha;
            _context.SaveChanges();
            return Ok("Dados atualizados");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Gestao_veiculos.Data;
using Gestao_veiculos.Models;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        public IActionResult Get()
        {
            var user = _context.User.ToList();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);

        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.User.Find(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.User.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, User updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest("O id da rota é diferente do id enviado no body.");
            }
            var user = _context.User.Find(id);

            if (user == null)
            {
                return NotFound("Usuario não encontrado");
            }
            user.Nome = updatedUser.Nome;
            user.Email = updatedUser.Email;
            user.Senha = updatedUser.Senha;
            _context.SaveChanges();
            return Ok("Dados atualizados");
        }
    }
}
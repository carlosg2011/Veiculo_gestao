using Gestao_veiculos.Data;
using Gestao_veiculos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProprietarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProprietarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proprietario>>> GetAll()
        {
            var proprietarios = await _context.Proprietarios.ToListAsync();
            return Ok(proprietarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Proprietario>> GetById(int id)
        {
            var proprietario = await _context.Proprietarios.FindAsync(id);

            if (proprietario == null)
                return NotFound("Proprietário não encontrado.");

            return Ok(proprietario);
        }

        [HttpPost]
        public async Task<ActionResult<Proprietario>> Create(Proprietario proprietario)
        {
            var cpfJaExiste = await _context.Proprietarios
                .AnyAsync(p => p.Cpf == proprietario.Cpf);

            if (cpfJaExiste)
                return BadRequest("Já existe um proprietário cadastrado com esse CPF.");

            _context.Proprietarios.Add(proprietario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = proprietario.Id_proprietario }, proprietario);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Proprietario proprietario)
        {
            if (id != proprietario.Id_proprietario)
                return BadRequest("Id da rota diferente do Id do objeto.");

            var proprietarioExistente = await _context.Proprietarios.FindAsync(id);

            if (proprietarioExistente == null)
                return NotFound("Proprietário não encontrado.");

            var cpfJaExiste = await _context.Proprietarios
                .AnyAsync(p => p.Cpf == proprietario.Cpf && p.Id_proprietario != id);

            if (cpfJaExiste)
                return BadRequest("Já existe outro proprietário cadastrado com esse CPF.");

            proprietarioExistente.Nome = proprietario.Nome;
            proprietarioExistente.Cpf = proprietario.Cpf;
            proprietarioExistente.Telefone = proprietario.Telefone;
            proprietarioExistente.Email = proprietario.Email;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var proprietario = await _context.Proprietarios.FindAsync(id);

            if (proprietario == null)
                return NotFound("Proprietário não encontrado.");

            _context.Proprietarios.Remove(proprietario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
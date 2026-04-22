using Gestao_veiculos.Data;
using Gestao_veiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropostaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PropostaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proposta>>> GetAll()
        {
            var propostas = await _context.Propostas.ToListAsync();
            return Ok(propostas);
        }

        [HttpGet("{id_proposta:int}")]
        public async Task<ActionResult<Proposta>> GetById(int id_proposta)
        {
            var proposta = await _context.Propostas.FindAsync(id_proposta);

            if (proposta == null)
                return NotFound("Proposta não encontrada.");

            return Ok(proposta);
        }

        [HttpPost]
        public async Task<ActionResult<Proposta>> Create(Proposta proposta)
        {
            var usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.Id_usuario == proposta.Id_usuario);

            if (!usuarioExiste)
                return BadRequest("Usuário informado não existe.");

            var proprietarioExiste = await _context.Proprietarios
                .AnyAsync(p => p.Id_proprietario == proposta.Id_proprietario);

            if (!proprietarioExiste)
                return BadRequest("Proprietário informado não existe.");

            var veiculoExiste = await _context.Veiculos
                .AnyAsync(v => v.Id_veiculo == proposta.Id_veiculo);

            if (!veiculoExiste)
                return BadRequest("Veículo informado não existe.");

            var codigoJaExiste = await _context.Propostas
                .AnyAsync(p => p.sessao_proposta == proposta.sessao_proposta);

            if (codigoJaExiste)
                return BadRequest("Já existe uma proposta cadastrada com esse código.");

            proposta.Data_Criacao = DateTime.Now;

            _context.Propostas.Add(proposta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id_proposta = proposta.Id_proposta }, proposta);
        }

        [HttpPut("{id_proposta:int}")]
        public async Task<ActionResult> Update(int id_proposta, Proposta proposta)
        {
            if (id_proposta != proposta.Id_proposta)
                return BadRequest("O id da rota é diferente do id da proposta.");

            var propostaExistente = await _context.Propostas.FindAsync(id_proposta);

            if (propostaExistente == null)
                return NotFound("Proposta não encontrada.");

            var usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.Id_usuario == proposta.Id_usuario);

            if (!usuarioExiste)
                return BadRequest("Usuário informado não existe.");

            var proprietarioExiste = await _context.Proprietarios
                .AnyAsync(p => p.Id_proprietario == proposta.Id_proprietario);

            if (!proprietarioExiste)
                return BadRequest("Proprietário informado não existe.");

            var veiculoExiste = await _context.Veiculos
                .AnyAsync(v => v.Id_veiculo == proposta.Id_veiculo);

            if (!veiculoExiste)
                return BadRequest("Veículo informado não existe.");

            var codigoJaExiste = await _context.Propostas
                .AnyAsync(p => p.sessao_proposta == proposta.sessao_proposta
                            && p.Id_proposta != id_proposta);

            if (codigoJaExiste)
                return BadRequest("Já existe outra proposta cadastrada com esse código.");

            propostaExistente.sessao_proposta = proposta.sessao_proposta;
            propostaExistente.Status = proposta.Status;
            propostaExistente.Id_usuario = proposta.Id_usuario;
            propostaExistente.Id_proprietario = proposta.Id_proprietario;
            propostaExistente.Id_veiculo = proposta.Id_veiculo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id_proposta:int}")]
        public async Task<ActionResult> Delete(int id_proposta)
        {
            var proposta = await _context.Propostas.FindAsync(id_proposta);

            if (proposta == null)
                return NotFound("Proposta não encontrada.");

            _context.Propostas.Remove(proposta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
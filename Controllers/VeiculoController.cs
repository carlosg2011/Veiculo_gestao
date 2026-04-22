using Gestao_veiculos.Data;
using Gestao_veiculos.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VeiculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var veiculos = _context.Veiculos.ToList();
            return Ok(veiculos);
        }

        [HttpGet("{id_veiculo}")]
        public IActionResult GetById(int id_veiculo)
        {
            var veiculo = _context.Veiculos.Find(id_veiculo);

            if (veiculo == null)
                return NotFound();

            return Ok(veiculo);
        }

        [HttpPost]
        public IActionResult Post(Veiculo veiculo)
        {
            _context.Veiculos.Add(veiculo);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id_veiculo = veiculo.Id_veiculo }, veiculo);
        }

        [HttpDelete("{id_veiculo}")]
        public IActionResult Delete(int id_veiculo)
        {
            var veiculo = _context.Veiculos.Find(id_veiculo);

            if (veiculo == null)
                return NotFound();

            _context.Veiculos.Remove(veiculo);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
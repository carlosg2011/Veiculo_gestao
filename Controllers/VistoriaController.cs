using Gestao_veiculos.Data;
using Gestao_veiculos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VistoriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VistoriaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var vistorias = _context.Vistorias.ToList();
            return Ok(vistorias);
        }

        [HttpGet("{id_vistoria}")]
        public IActionResult GetById(int id_vistoria)
        {
            var vistoria = _context.Vistorias.Find(id_vistoria);

            if (vistoria == null)
                return NotFound();

            return Ok(vistoria);
        }

        [HttpPost]
        public IActionResult Post(Vistoria vistoria)
        {
            _context.Vistorias.Add(vistoria);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id_vistoria = vistoria.Id_vistoria }, vistoria);
        }

        [HttpDelete("{id_vistoria}")]
        public IActionResult Delete(int id_vistoria)
        {
            var vistoria = _context.Vistorias.Find(id_vistoria);

            if (vistoria == null)
                return NotFound();

            _context.Vistorias.Remove(vistoria);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
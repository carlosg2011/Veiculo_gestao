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
    public class TermosController : ControllerBase
    {

        private readonly AppDbContext _context;

        public TermosController(AppDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        public IActionResult Get()
        {
            var termo = _context.Termos.ToList();
            return Ok(termo);
        }

        [HttpPost]
        public IActionResult Post(Termo termo)
        {
            _context.Termos.Add(termo);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id_termo = termo.Id_termo }, termo);

        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id_termo)
        {
            var termo = _context.Termos.Find(id_termo);

            if (termo == null)
                return NotFound();

            return Ok(termo);
        }

        [HttpDelete("{id_termo}")]
        public IActionResult Delete(int id_termo)
        {
            var termo = _context.Termos.Find(id_termo);
            if (termo == null)
            {
                return NotFound();
            }
            _context.Termos.Remove(termo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TermosController : ControllerBase
    {
        private readonly ITermoService _service;

        public TermosController(ITermoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_service.ListarTodos());

        [HttpGet("{id_termo}")]
        public IActionResult GetById(int id_termo)
        {
            var termo = _service.BuscarPorId(id_termo);
            return termo is null ? NotFound() : Ok(termo);
        }

        [HttpPost]
        public IActionResult Post(CreateTermoDto dto)
        {
            try
            {
                var termo = _service.Criar(dto);
                return CreatedAtAction(nameof(GetById), new { id_termo = termo.Id_termo }, termo);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id_termo}")]
        public IActionResult Put(int id_termo, CreateTermoDto dto)
        {
            try
            {
                var termo = _service.Atualizar(id_termo, dto);
                return Ok(termo);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id_termo}")]
        public IActionResult Delete(int id_termo)
        {
            try
            {
                _service.Deletar(id_termo);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

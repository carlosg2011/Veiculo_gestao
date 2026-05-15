using Gestao_veiculos.DTOs;
using Gestao_veiculos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProprietarioController : ControllerBase
    {
        private readonly IProprietarioService _service;

        public ProprietarioController(IProprietarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_service.ListarTodos());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var proprietario = _service.BuscarPorId(id);
            return proprietario is null ? NotFound() : Ok(proprietario);
        }

        [HttpPost]
        public IActionResult Post(CreateProprietarioDto dto)
        {
            try
            {
                var proprietario = _service.Criar(dto);
                return CreatedAtAction(nameof(GetById), new { id = proprietario.Id_proprietario }, proprietario);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, CreateProprietarioDto dto)
        {
            try
            {
                var proprietario = _service.Atualizar(id, dto);
                return Ok(proprietario);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.Deletar(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

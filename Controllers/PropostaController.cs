using Gestao_veiculos.DTOs;
using Gestao_veiculos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PropostaController : ControllerBase
    {
        private readonly IPropostaService _service;

        public PropostaController(IPropostaService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_service.ListarTodos());

        [HttpGet("{id_proposta:int}")]
        public IActionResult GetById(int id_proposta)
        {
            var proposta = _service.BuscarPorId(id_proposta);
            return proposta is null ? NotFound() : Ok(proposta);
        }

        [HttpPost]
        public IActionResult Post(CreatePropostaDto dto)
        {
            try
            {
                var proposta = _service.Criar(dto);
                return CreatedAtAction(nameof(GetById), new { id_proposta = proposta.Id_proposta }, proposta);
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

        [HttpPut("{id_proposta:int}")]
        public IActionResult Put(int id_proposta, CreatePropostaDto dto)
        {
            try
            {
                var proposta = _service.Atualizar(id_proposta, dto);
                return Ok(proposta);
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

        [HttpDelete("{id_proposta:int}")]
        public IActionResult Delete(int id_proposta)
        {
            try
            {
                _service.Deletar(id_proposta);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

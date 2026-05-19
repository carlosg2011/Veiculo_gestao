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
        public async Task<IActionResult> Get([FromQuery] PaginationParams pagination) =>
            Ok(await _service.ListarTodos(pagination));

        [HttpGet("{id_proposta:int}")]
        public async Task<IActionResult> GetById(int id_proposta)
        {
            var proposta = await _service.BuscarPorId(id_proposta);
            return proposta is null
                ? Problem(statusCode: StatusCodes.Status404NotFound)
                : Ok(proposta);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreatePropostaDto dto)
        {
            try
            {
                var proposta = await _service.Criar(dto);
                return CreatedAtAction(nameof(GetById), new { id_proposta = proposta.Id_proposta }, proposta);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
            catch (InvalidOperationException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id_proposta:int}")]
        public async Task<IActionResult> Put(int id_proposta, CreatePropostaDto dto)
        {
            try
            {
                var proposta = await _service.Atualizar(id_proposta, dto);
                return Ok(proposta);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
            catch (InvalidOperationException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpDelete("{id_proposta:int}")]
        public async Task<IActionResult> Delete(int id_proposta)
        {
            try
            {
                await _service.Deletar(id_proposta);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
        }
    }
}

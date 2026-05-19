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
        public async Task<IActionResult> Get() => Ok(await _service.ListarTodos());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var proprietario = await _service.BuscarPorId(id);
            return proprietario is null
                ? Problem(statusCode: StatusCodes.Status404NotFound)
                : Ok(proprietario);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProprietarioDto dto)
        {
            try
            {
                var proprietario = await _service.Criar(dto);
                return CreatedAtAction(nameof(GetById), new { id = proprietario.Id_proprietario }, proprietario);
            }
            catch (InvalidOperationException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CreateProprietarioDto dto)
        {
            try
            {
                var proprietario = await _service.Atualizar(id, dto);
                return Ok(proprietario);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Deletar(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
        }
    }
}

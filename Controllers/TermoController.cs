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
        public async Task<IActionResult> Get([FromQuery] PaginationParams pagination, [FromQuery] int? userId) =>
            Ok(await _service.ListarTodos(pagination, userId));

        [HttpGet("{id_termo}")]
        public async Task<IActionResult> GetById(int id_termo)
        {
            var termo = await _service.BuscarPorId(id_termo);
            return termo is null
                ? Problem(statusCode: StatusCodes.Status404NotFound)
                : Ok(termo);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateTermoDto dto)
        {
            try
            {
                var termo = await _service.Criar(dto);
                return CreatedAtAction(nameof(GetById), new { id_termo = termo.Id_termo }, termo);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
        }

        [HttpPut("{id_termo}")]
        public async Task<IActionResult> Put(int id_termo, CreateTermoDto dto)
        {
            try
            {
                var termo = await _service.Atualizar(id_termo, dto);
                return Ok(termo);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
        }

        [HttpDelete("{id_termo}")]
        public async Task<IActionResult> Delete(int id_termo)
        {
            try
            {
                await _service.Deletar(id_termo);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
        }
    }
}

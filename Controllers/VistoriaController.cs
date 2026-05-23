using Gestao_veiculos.DTOs;
using Gestao_veiculos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VistoriaController : ControllerBase
    {
        private readonly IVistoriaService _service;

        public VistoriaController(IVistoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationParams pagination, [FromQuery] int? userId, [FromQuery] int? propostaId) =>
            Ok(await _service.ListarTodos(pagination, userId, propostaId));

        [HttpGet("{id_vistoria}")]
        public async Task<IActionResult> GetById(int id_vistoria)
        {
            var vistoria = await _service.BuscarPorId(id_vistoria);
            return vistoria is null
                ? Problem(statusCode: StatusCodes.Status404NotFound)
                : Ok(vistoria);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateVistoriaDto dto)
        {
            try
            {
                var vistoria = await _service.Criar(dto);
                return CreatedAtAction(nameof(GetById), new { id_vistoria = vistoria.Id_vistoria }, vistoria);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
        }

        [HttpPut("{id_vistoria}")]
        public async Task<IActionResult> Put(int id_vistoria, UpdateVistoriaDto dto)
        {
            try
            {
                var vistoria = await _service.Atualizar(id_vistoria, dto);
                return Ok(vistoria);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
        }

        [HttpDelete("{id_vistoria}")]
        public async Task<IActionResult> Delete(int id_vistoria)
        {
            try
            {
                await _service.Deletar(id_vistoria);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
        }
    }
}

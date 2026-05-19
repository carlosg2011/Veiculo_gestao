using Gestao_veiculos.DTOs;
using Gestao_veiculos.Enums;
using Gestao_veiculos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VeiculosController : ControllerBase
    {
        private readonly IVeiculoService _service;

        public VeiculosController(IVeiculoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.ListarTodos());

        [HttpGet("{id_veiculo}")]
        public async Task<IActionResult> GetById(int id_veiculo)
        {
            var veiculo = await _service.BuscarPorId(id_veiculo);
            return veiculo is null
                ? Problem(statusCode: StatusCodes.Status404NotFound)
                : Ok(veiculo);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateVeiculoDto dto)
        {
            try
            {
                var veiculo = await _service.Criar(dto);
                return CreatedAtAction(nameof(GetById), new { id_veiculo = veiculo.Id_veiculo }, veiculo);
            }
            catch (InvalidOperationException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id_veiculo}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Put(int id_veiculo, CreateVeiculoDto dto)
        {
            try
            {
                var veiculo = await _service.Atualizar(id_veiculo, dto);
                return Ok(veiculo);
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

        [HttpDelete("{id_veiculo}")]
        public async Task<IActionResult> Delete(int id_veiculo)
        {
            try
            {
                await _service.Deletar(id_veiculo);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
        }
    }
}

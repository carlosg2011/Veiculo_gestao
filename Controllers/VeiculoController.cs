using Gestao_veiculos.DTOs;
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
        public IActionResult Get() => Ok(_service.ListarTodos());

        [HttpGet("{id_veiculo}")]
        public IActionResult GetById(int id_veiculo)
        {
            var veiculo = _service.BuscarPorId(id_veiculo);
            return veiculo is null ? NotFound() : Ok(veiculo);
        }

        [HttpPost]
        public IActionResult Post(CreateVeiculoDto dto)
        {
            try
            {
                var veiculo = _service.Criar(dto);
                return CreatedAtAction(nameof(GetById), new { id_veiculo = veiculo.Id_veiculo }, veiculo);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id_veiculo}")]
        public IActionResult Put(int id_veiculo, CreateVeiculoDto dto)
        {
            try
            {
                var veiculo = _service.Atualizar(id_veiculo, dto);
                return Ok(veiculo);
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

        [HttpDelete("{id_veiculo}")]
        public IActionResult Delete(int id_veiculo)
        {
            try
            {
                _service.Deletar(id_veiculo);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

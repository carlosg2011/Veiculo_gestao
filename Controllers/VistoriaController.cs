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
        public IActionResult Get() => Ok(_service.ListarTodos());

        [HttpGet("{id_vistoria}")]
        public IActionResult GetById(int id_vistoria)
        {
            var vistoria = _service.BuscarPorId(id_vistoria);
            return vistoria is null ? NotFound() : Ok(vistoria);
        }

        [HttpPost]
        public IActionResult Post(CreateVistoriaDto dto)
        {
            try
            {
                var vistoria = _service.Criar(dto);
                return CreatedAtAction(nameof(GetById), new { id_vistoria = vistoria.Id_vistoria }, vistoria);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id_vistoria}")]
        public IActionResult Put(int id_vistoria, UpdateVistoriaDto dto)
        {
            try
            {
                var vistoria = _service.Atualizar(id_vistoria, dto);
                return Ok(vistoria);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id_vistoria}")]
        public IActionResult Delete(int id_vistoria)
        {
            try
            {
                _service.Deletar(id_vistoria);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

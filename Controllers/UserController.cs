using Gestao_veiculos.DTOs;
using Gestao_veiculos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.ListarTodos());
        }

        [HttpGet("{id_usuario}")]
        public IActionResult GetById(int id_usuario)
        {
            var usuario = _service.BuscarPorId(id_usuario);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult Post(CreateUserDto dto)
        {
            try
            {
                var usuario = _service.Criar(dto);
                return CreatedAtAction(nameof(GetById), new { id_usuario = usuario.Id_usuario }, usuario);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id_usuario}")]
        public IActionResult Put(int id_usuario, CreateUserDto dto)
        {
            try
            {
                var usuario = _service.Atualizar(id_usuario, dto);
                return Ok(usuario);
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

        [HttpDelete("{id_usuario}")]
        public IActionResult Delete(int id_usuario)
        {
            try
            {
                _service.Deletar(id_usuario);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
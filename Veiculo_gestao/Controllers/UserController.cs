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
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationParams pagination) =>
            Ok(await _service.ListarTodos(pagination));

        [HttpGet("{id_usuario}")]
        public async Task<IActionResult> GetById(int id_usuario)
        {
            var usuario = await _service.BuscarPorId(id_usuario);
            return usuario is null
                ? Problem(statusCode: StatusCodes.Status404NotFound)
                : Ok(usuario);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Post(CreateUserDto dto)
        {
            try
            {
                var usuario = await _service.Criar(dto);
                return CreatedAtAction(nameof(GetById), new { id_usuario = usuario.Id_usuario }, usuario);
            }
            catch (InvalidOperationException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id_usuario}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Put(int id_usuario, UpdateUserDto dto)
        {
            try
            {
                var usuario = await _service.Atualizar(id_usuario, dto);
                return Ok(usuario);
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

        [HttpDelete("{id_usuario}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id_usuario)
        {
            try
            {
                await _service.Deletar(id_usuario);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
        }
    }
}

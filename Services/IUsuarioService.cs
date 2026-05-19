using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface IUsuarioService
    {
        Task<PagedResultDto<ResponseUserDto>> ListarTodos(PaginationParams pagination);
        Task<ResponseUserDto?> BuscarPorId(int id);
        Task<ResponseUserDto> Criar(CreateUserDto dto);
        Task<ResponseUserDto> Atualizar(int id, UpdateUserDto dto);
        Task Deletar(int id);
    }
}

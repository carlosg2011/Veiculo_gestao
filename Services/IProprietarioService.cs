using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface IProprietarioService
    {
        Task<PagedResultDto<ResponseProprietarioDto>> ListarTodos(PaginationParams pagination);
        Task<ResponseProprietarioDto?> BuscarPorId(int id);
        Task<ResponseProprietarioDto> Criar(CreateProprietarioDto dto);
        Task<ResponseProprietarioDto> Atualizar(int id, CreateProprietarioDto dto);
        Task Deletar(int id);
    }
}

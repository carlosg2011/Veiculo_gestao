using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface ITermoService
    {
        Task<PagedResultDto<ResponseTermoDto>> ListarTodos(PaginationParams pagination);
        Task<ResponseTermoDto?> BuscarPorId(int id);
        Task<ResponseTermoDto> Criar(CreateTermoDto dto);
        Task<ResponseTermoDto> Atualizar(int id, CreateTermoDto dto);
        Task Deletar(int id);
    }
}

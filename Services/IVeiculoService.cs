using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface IVeiculoService
    {
        Task<PagedResultDto<ResponseVeiculoDto>> ListarTodos(PaginationParams pagination);
        Task<ResponseVeiculoDto?> BuscarPorId(int id);
        Task<ResponseVeiculoDto> Criar(CreateVeiculoDto dto);
        Task<ResponseVeiculoDto> Atualizar(int id, CreateVeiculoDto dto);
        Task Deletar(int id);
    }
}

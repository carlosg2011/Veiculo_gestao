using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface IVistoriaService
    {
        Task<PagedResultDto<ResponseVistoriaDto>> ListarTodos(PaginationParams pagination, int? userId = null, int? propostaId = null, bool excludeCancelled = false);
        Task<ResponseVistoriaDto?> BuscarPorId(int id);
        Task<ResponseVistoriaDto> Criar(CreateVistoriaDto dto);
        Task<ResponseVistoriaDto> Atualizar(int id, UpdateVistoriaDto dto);
        Task Deletar(int id);
    }
}

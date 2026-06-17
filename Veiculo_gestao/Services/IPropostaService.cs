using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface IPropostaService
    {
        Task<PagedResultDto<ResponsePropostaDto>> ListarTodos(PaginationParams pagination, int? userId = null);
        Task<PagedResultDto<ResponsePropostaCompletoDto>> Buscar(PaginationParams pagination, PropostaFiltroParams filtro);
        Task<ResponsePropostaDto?> BuscarPorId(int id);
        Task<ResponsePropostaDto> Criar(CreatePropostaDto dto);
        Task<ResponsePropostaDto> Atualizar(int id, CreatePropostaDto dto);
        Task Deletar(int id);
    }
}

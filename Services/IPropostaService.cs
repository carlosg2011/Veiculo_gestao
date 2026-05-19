using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface IPropostaService
    {
        Task<IEnumerable<ResponsePropostaDto>> ListarTodos();
        Task<ResponsePropostaDto?> BuscarPorId(int id);
        Task<ResponsePropostaDto> Criar(CreatePropostaDto dto);
        Task<ResponsePropostaDto> Atualizar(int id, CreatePropostaDto dto);
        Task Deletar(int id);
    }
}

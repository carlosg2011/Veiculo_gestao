using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface IPropostaService
    {
        IEnumerable<ResponsePropostaDto> ListarTodos();
        ResponsePropostaDto? BuscarPorId(int id);
        ResponsePropostaDto Criar(CreatePropostaDto dto);
        ResponsePropostaDto Atualizar(int id, CreatePropostaDto dto);
        void Deletar(int id);
    }
}

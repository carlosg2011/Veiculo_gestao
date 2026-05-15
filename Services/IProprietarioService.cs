using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface IProprietarioService
    {
        IEnumerable<ResponseProprietarioDto> ListarTodos();
        ResponseProprietarioDto? BuscarPorId(int id);
        ResponseProprietarioDto Criar(CreateProprietarioDto dto);
        ResponseProprietarioDto Atualizar(int id, CreateProprietarioDto dto);
        void Deletar(int id);
    }
}

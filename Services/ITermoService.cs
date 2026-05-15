using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface ITermoService
    {
        IEnumerable<ResponseTermoDto> ListarTodos();
        ResponseTermoDto? BuscarPorId(int id);
        ResponseTermoDto Criar(CreateTermoDto dto);
        ResponseTermoDto Atualizar(int id, CreateTermoDto dto);
        void Deletar(int id);
    }
}

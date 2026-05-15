using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface IVeiculoService
    {
        IEnumerable<ResponseVeiculoDto> ListarTodos();
        ResponseVeiculoDto? BuscarPorId(int id);
        ResponseVeiculoDto Criar(CreateVeiculoDto dto);
        ResponseVeiculoDto Atualizar(int id, CreateVeiculoDto dto);
        void Deletar(int id);
    }
}

using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface IVeiculoService
    {
        Task<IEnumerable<ResponseVeiculoDto>> ListarTodos();
        Task<ResponseVeiculoDto?> BuscarPorId(int id);
        Task<ResponseVeiculoDto> Criar(CreateVeiculoDto dto);
        Task<ResponseVeiculoDto> Atualizar(int id, CreateVeiculoDto dto);
        Task Deletar(int id);
    }
}

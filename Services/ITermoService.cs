using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface ITermoService
    {
        Task<IEnumerable<ResponseTermoDto>> ListarTodos();
        Task<ResponseTermoDto?> BuscarPorId(int id);
        Task<ResponseTermoDto> Criar(CreateTermoDto dto);
        Task<ResponseTermoDto> Atualizar(int id, CreateTermoDto dto);
        Task Deletar(int id);
    }
}

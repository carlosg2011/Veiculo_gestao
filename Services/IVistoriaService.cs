using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface IVistoriaService
    {
        Task<IEnumerable<ResponseVistoriaDto>> ListarTodos();
        Task<ResponseVistoriaDto?> BuscarPorId(int id);
        Task<ResponseVistoriaDto> Criar(CreateVistoriaDto dto);
        Task<ResponseVistoriaDto> Atualizar(int id, UpdateVistoriaDto dto);
        Task Deletar(int id);
    }
}

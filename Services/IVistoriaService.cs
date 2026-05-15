using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface IVistoriaService
    {
        IEnumerable<ResponseVistoriaDto> ListarTodos();
        ResponseVistoriaDto? BuscarPorId(int id);
        ResponseVistoriaDto Criar(CreateVistoriaDto dto);
        ResponseVistoriaDto Atualizar(int id, UpdateVistoriaDto dto);
        void Deletar(int id);
    }
}

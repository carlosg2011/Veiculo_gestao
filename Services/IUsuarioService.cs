using Gestao_veiculos.DTOs;

namespace Gestao_veiculos.Services
{
    public interface IUsuarioService
    {
        IEnumerable<ResponseUserDto> ListarTodos();
        ResponseUserDto? BuscarPorId(int id);
        ResponseUserDto Criar(CreateUserDto dto);
        ResponseUserDto Atualizar(int id, CreateUserDto dto);
        void Deletar(int id);
    }
}

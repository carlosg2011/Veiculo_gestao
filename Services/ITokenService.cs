using Gestao_veiculos.Models;

namespace Gestao_veiculos.Services
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
    }
}

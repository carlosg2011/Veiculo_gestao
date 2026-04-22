using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Gestao_veiculos.Models
{
    public class Proprietario
    {
        public int Id_proprietario { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
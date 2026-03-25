using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Gestao_veiculos.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Nome { get; set; }

        public string Email { get; set; }
        public string Senha { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace Gestao_veiculos.DTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(80, ErrorMessage = "Nome deve ter no máximo 80 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

        public string? Senha { get; set; }

        [Required(ErrorMessage = "Role é obrigatório.")]
        [RegularExpression("^(Admin|User)$", ErrorMessage = "Role deve ser 'Admin' ou 'User'.")]
        public string Role { get; set; } = "User";
    }
}

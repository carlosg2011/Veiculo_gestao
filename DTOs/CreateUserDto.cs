using System.ComponentModel.DataAnnotations;

namespace Gestao_veiculos.DTOs
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(80, ErrorMessage = "Nome pode ter no máximo 80 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório.")]
        [MaxLength(100, ErrorMessage = "Email pode ter no máximo 100 caracteres.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "Senha deve ter no mínimo 8 caracteres.")]
        [MaxLength(255, ErrorMessage = "Senha pode ter no máximo 255 caracteres.")]
        public string Senha { get; set; } = string.Empty;
    }
}

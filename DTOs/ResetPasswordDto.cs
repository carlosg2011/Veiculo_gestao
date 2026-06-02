using System.ComponentModel.DataAnnotations;

namespace Gestao_veiculos.DTOs
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Token é obrigatório.")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nova senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter ao menos 6 caracteres.")]
        public string NovaSenha { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Gestao_veiculos.DTOs
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;
    }
}

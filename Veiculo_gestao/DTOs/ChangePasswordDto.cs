using System.ComponentModel.DataAnnotations;

namespace Gestao_veiculos.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Senha atual é obrigatória.")]
        public string SenhaAtual { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nova senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "Nova senha deve ter no mínimo 8 caracteres.")]
        public string NovaSenha { get; set; } = string.Empty;
    }
}
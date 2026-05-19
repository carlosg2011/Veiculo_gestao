using System.ComponentModel.DataAnnotations;

namespace Gestao_veiculos.DTOs
{
    public class CreateProprietarioDto
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(80, ErrorMessage = "Nome pode ter no máximo 80 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "CPF é obrigatório.")]
        [MaxLength(14, ErrorMessage = "CPF pode ter no máximo 14 caracteres.")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório.")]
        [MaxLength(15, ErrorMessage = "Telefone pode ter no máximo 15 caracteres.")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório.")]
        [MaxLength(100, ErrorMessage = "Email pode ter no máximo 100 caracteres.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;
    }
}

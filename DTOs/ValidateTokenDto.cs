using System.ComponentModel.DataAnnotations;

namespace Gestao_veiculos.DTOs
{
    public class ValidateTokenDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}

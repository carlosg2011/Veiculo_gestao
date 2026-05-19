using System.ComponentModel.DataAnnotations;
using Gestao_veiculos.Enums;

namespace Gestao_veiculos.DTOs
{
    public class CreateTermoDto
    {
        [Required(ErrorMessage = "Número do termo é obrigatório.")]
        [MaxLength(25, ErrorMessage = "Número do termo pode ter no máximo 25 caracteres.")]
        public string NumeroTermo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status é obrigatório.")]
        public StatusTermo? Status { get; set; }

        [Required(ErrorMessage = "Data de envio é obrigatória.")]
        public DateTime DataEnvio { get; set; }

        public DateTime? DataAssinatura { get; set; }

        [Required(ErrorMessage = "Id da proposta é obrigatório.")]
        public int Id_proposta { get; set; }
    }
}

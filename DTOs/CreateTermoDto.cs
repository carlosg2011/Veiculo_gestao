using System.ComponentModel.DataAnnotations;

namespace Gestao_veiculos.DTOs
{
    public class CreateTermoDto
    {
        [Required(ErrorMessage = "Número do termo é obrigatório.")]
        [MaxLength(25, ErrorMessage = "Número do termo pode ter no máximo 25 caracteres.")]
        public string Numero_termo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status é obrigatório.")]
        [MaxLength(30, ErrorMessage = "Status pode ter no máximo 30 caracteres.")]
        public string Status_termo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data de envio é obrigatória.")]
        public DateTime Data_envio { get; set; }

        public DateTime? Data_assinatura { get; set; }

        [Required(ErrorMessage = "Id da proposta é obrigatório.")]
        public int Id_proposta { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Gestao_veiculos.DTOs
{
    public class UpdateVistoriaDto
    {
        [Required(ErrorMessage = "Status é obrigatório.")]
        [MaxLength(30, ErrorMessage = "Status pode ter no máximo 30 caracteres.")]
        public string Status_vistoria { get; set; } = string.Empty;

        public DateTime? Data_inicio { get; set; }
        public DateTime? Data_conclusao { get; set; }
    }
}

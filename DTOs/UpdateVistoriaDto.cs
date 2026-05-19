using System.ComponentModel.DataAnnotations;
using Gestao_veiculos.Enums;

namespace Gestao_veiculos.DTOs
{
    public class UpdateVistoriaDto
    {
        [Required(ErrorMessage = "Status é obrigatório.")]
        public StatusVistoria? Status_vistoria { get; set; }

        public DateTime? Data_inicio { get; set; }
        public DateTime? Data_conclusao { get; set; }
    }
}

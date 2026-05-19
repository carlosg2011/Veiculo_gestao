using System.ComponentModel.DataAnnotations;
using Gestao_veiculos.Enums;

namespace Gestao_veiculos.DTOs
{
    public class UpdateVistoriaDto
    {
        [Required(ErrorMessage = "Status é obrigatório.")]
        public StatusVistoria? Status { get; set; }

        public DateTime? DataInicio { get; set; }
        public DateTime? DataConclusao { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Gestao_veiculos.Enums;

namespace Gestao_veiculos.DTOs
{
    public class CreateVistoriaDto
    {
        [Required(ErrorMessage = "Data de solicitação é obrigatória.")]
        public DateTime Data_solicitacao { get; set; }

        [Required(ErrorMessage = "Status é obrigatório.")]
        public StatusVistoria? Status_vistoria { get; set; }

        [Required(ErrorMessage = "Id da proposta é obrigatório.")]
        public int Id_proposta { get; set; }

        [Required(ErrorMessage = "Id do usuário responsável é obrigatório.")]
        public int Id_usuario { get; set; }
    }
}

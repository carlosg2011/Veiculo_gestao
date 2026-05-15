using System.ComponentModel.DataAnnotations;

namespace Gestao_veiculos.DTOs
{
    public class CreateVistoriaDto
    {
        [Required(ErrorMessage = "Data de solicitação é obrigatória.")]
        public DateTime Data_solicitacao { get; set; }

        [Required(ErrorMessage = "Status é obrigatório.")]
        [MaxLength(30, ErrorMessage = "Status pode ter no máximo 30 caracteres.")]
        public string Status_vistoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "Id da proposta é obrigatório.")]
        public int Id_proposta { get; set; }

        [Required(ErrorMessage = "Id do usuário responsável é obrigatório.")]
        public int Id_usuario { get; set; }
    }
}

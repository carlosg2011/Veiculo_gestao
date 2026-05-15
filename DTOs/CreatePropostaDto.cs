using System.ComponentModel.DataAnnotations;

namespace Gestao_veiculos.DTOs
{
    public class CreatePropostaDto
    {
        [Required(ErrorMessage = "Código da proposta é obrigatório.")]
        [MaxLength(25, ErrorMessage = "Código da proposta pode ter no máximo 25 caracteres.")]
        public string Sessao_proposta { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status é obrigatório.")]
        [MaxLength(30, ErrorMessage = "Status pode ter no máximo 30 caracteres.")]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "Id do usuário é obrigatório.")]
        public int Id_usuario { get; set; }

        [Required(ErrorMessage = "Id do veículo é obrigatório.")]
        public int Id_veiculo { get; set; }

        [Required(ErrorMessage = "Id do proprietário é obrigatório.")]
        public int Id_proprietario { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Gestao_veiculos.Enums;

namespace Gestao_veiculos.DTOs
{
    public class CreateVeiculoDto
    {
        [Required(ErrorMessage = "Placa é obrigatória.")]
        [RegularExpression(@"^[A-Z]{3}[0-9]{4}$|^[A-Z]{3}[0-9][A-Z][0-9]{2}$",
            ErrorMessage = "Placa inválida. Use o formato ABC1234 (antigo) ou ABC1D23 (Mercosul).")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "Marca é obrigatória.")]
        [MaxLength(50, ErrorMessage = "Marca pode ter no máximo 50 caracteres.")]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "Modelo é obrigatório.")]
        [MaxLength(50, ErrorMessage = "Modelo pode ter no máximo 50 caracteres.")]
        public string Modelo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ano de fabricação é obrigatório.")]
        [Range(1900, 2100, ErrorMessage = "Ano de fabricação inválido.")]
        public int AnoFab { get; set; }

        [Required(ErrorMessage = "Ano do modelo é obrigatório.")]
        [Range(1900, 2100, ErrorMessage = "Ano do modelo inválido.")]
        public int AnoMod { get; set; }

        [Required(ErrorMessage = "Chassi é obrigatório.")]
        [MaxLength(17, ErrorMessage = "Chassi pode ter no máximo 17 caracteres.")]
        public string Chassi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Renavam é obrigatório.")]
        [MaxLength(11, ErrorMessage = "Renavam pode ter no máximo 11 caracteres.")]
        public string Renavam { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cor é obrigatória.")]
        [MaxLength(20, ErrorMessage = "Cor pode ter no máximo 20 caracteres.")]
        public string Cor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status é obrigatório.")]
        public StatusVeiculo? Status { get; set; }
    }
}

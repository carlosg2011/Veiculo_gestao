using Gestao_veiculos.Enums;

namespace Gestao_veiculos.Models
{
    public class Proposta
    {
        public int Id_proposta { get; set; }
        public string SessaoProposta { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public StatusProposta Status { get; set; }
        public int Id_usuario { get; set; }
        public int Id_veiculo { get; set; }
        public int Id_proprietario { get; set; }
    }
}

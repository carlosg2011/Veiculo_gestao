using Gestao_veiculos.Enums;

namespace Gestao_veiculos.Models
{
    public class Vistoria
    {
        public int Id_vistoria { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataConclusao { get; set; }
        public StatusVistoria Status { get; set; }
        public int Id_proposta { get; set; }
        public int Id_usuario { get; set; }
    }
}

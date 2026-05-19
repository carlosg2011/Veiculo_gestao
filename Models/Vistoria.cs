using Gestao_veiculos.Enums;

namespace Gestao_veiculos.Models
{
    public class Vistoria
    {
        public int Id_vistoria { get; set; }
        public DateTime Data_solicitacao { get; set; }
        public DateTime? data_inicio { get; set; }
        public DateTime? data_conclusao { get; set; }
        public StatusVistoria status_vistoria { get; set; }
        public int Id_proposta { get; set; }
        public int Id_usuario { get; set; }
    }
}
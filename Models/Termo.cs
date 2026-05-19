using Gestao_veiculos.Enums;

namespace Gestao_veiculos.Models
{
    public class Termo
    {
        public int Id_termo { get; set; }
        public string numero_termo { get; set; } = string.Empty;
        public StatusTermo status_termo { get; set; }
        public DateTime data_envio { get; set; }
        public DateTime? data_assinatura { get; set; }
        public int Id_proposta { get; set; }
    }
}

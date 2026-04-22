using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Gestao_veiculos.Models
{
    public class Termo
    {
        public int Id_termo { get; set; }
        public string numero_termo { get; set; } = string.Empty;
        public string status_termo { get; set; } = string.Empty;
        public DateTime data_envio { get; set; }
        public DateTime data_assinatura { get; set; }
        public int Id_proposta { get; set; }

    }
}   
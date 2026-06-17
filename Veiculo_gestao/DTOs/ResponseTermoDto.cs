using Gestao_veiculos.Enums;

namespace Gestao_veiculos.DTOs
{
    public class ResponseTermoDto
    {
        public int Id_termo { get; set; }
        public string NumeroTermo { get; set; } = string.Empty;
        public StatusTermo Status { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime? DataAssinatura { get; set; }
        public int Id_proposta { get; set; }
    }
}

namespace Gestao_veiculos.DTOs
{
    public class ResponseTermoDto
    {
        public int Id_termo { get; set; }
        public string Numero_termo { get; set; } = string.Empty;
        public string Status_termo { get; set; } = string.Empty;
        public DateTime Data_envio { get; set; }
        public DateTime? Data_assinatura { get; set; }
        public int Id_proposta { get; set; }
    }
}

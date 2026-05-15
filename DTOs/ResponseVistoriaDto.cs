namespace Gestao_veiculos.DTOs
{
    public class ResponseVistoriaDto
    {
        public int Id_vistoria { get; set; }
        public DateTime Data_solicitacao { get; set; }
        public DateTime? Data_inicio { get; set; }
        public DateTime? Data_conclusao { get; set; }
        public string Status_vistoria { get; set; } = string.Empty;
        public int Id_proposta { get; set; }
        public int Id_usuario { get; set; }
    }
}

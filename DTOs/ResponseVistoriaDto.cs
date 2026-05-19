using Gestao_veiculos.Enums;

namespace Gestao_veiculos.DTOs
{
    public class ResponseVistoriaDto
    {
        public int Id_vistoria { get; set; }
        public DateTime Data_solicitacao { get; set; }
        public DateTime? Data_inicio { get; set; }
        public DateTime? Data_conclusao { get; set; }
        public StatusVistoria Status_vistoria { get; set; }
        public int Id_proposta { get; set; }
        public int Id_usuario { get; set; }
    }
}

using Gestao_veiculos.Enums;

namespace Gestao_veiculos.DTOs
{
    public class ResponseVistoriaDto
    {
        public int Id_vistoria { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataConclusao { get; set; }
        public StatusVistoria Status { get; set; }
        public int Id_proposta { get; set; }
        public string SessaoProposta { get; set; } = string.Empty;
        public int Id_usuario { get; set; }
        public string NomeProprietario { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
    }
}

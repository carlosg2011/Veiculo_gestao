using Gestao_veiculos.Enums;

namespace Gestao_veiculos.DTOs
{
    public class ResponsePropostaCompletoDto
    {
        public int Id_proposta { get; set; }
        public string SessaoProposta { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public StatusProposta StatusProposta { get; set; }
        public int Id_usuario { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;

        public int Id_proprietario { get; set; }
        public string NomeProprietario { get; set; } = string.Empty;
        public string CpfProprietario { get; set; } = string.Empty;

        public int Id_veiculo { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Chassi { get; set; } = string.Empty;
        public string Renavam { get; set; } = string.Empty;
        public StatusVeiculo StatusVeiculo { get; set; }
    }
}

using Gestao_veiculos.Enums;

namespace Gestao_veiculos.DTOs
{
    public class PropostaFiltroParams
    {
        public string? Nome { get; set; }
        public string? Placa { get; set; }
        public string? Renavam { get; set; }
        public string? Chassi { get; set; }
        public StatusVeiculo? StatusVeiculo { get; set; }
        public StatusProposta? StatusProposta { get; set; }
    }
}

using Gestao_veiculos.Enums;

namespace Gestao_veiculos.Models
{
    public class Veiculo
    {
        public int Id_veiculo { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int AnoFab { get; set; }
        public int AnoMod { get; set; }
        public string Chassi { get; set; } = string.Empty;
        public string Renavam { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public StatusVeiculo Status { get; set; }
    }
}
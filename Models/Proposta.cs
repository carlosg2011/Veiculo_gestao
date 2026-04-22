namespace Gestao_veiculos.Models
{
    public class Proposta
    {
        public int Id_proposta { get; set; }
        public string sessao_proposta { get; set; } = string.Empty;
        public DateTime Data_Criacao { get; set; }
        public string Status { get; set; } = string.Empty;
        public int Id_usuario { get; set; }
        public int Id_veiculo { get; set; }
        public int Id_proprietario { get; set; }
    }
}
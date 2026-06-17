namespace Gestao_veiculos.Models
{
    public class VistoriaFoto
    {
        public int Id_foto { get; set; }
        public int Id_vistoria { get; set; }
        public string Slot { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Verdict { get; set; }
    }
}

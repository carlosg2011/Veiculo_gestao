namespace Gestao_veiculos.DTOs
{
    public class VistoriaFotoDto
    {
        public int Id_foto { get; set; }
        public string Slot { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Verdict { get; set; }
    }

    public class UpsertVistoriaFotoDto
    {
        public string Slot { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Verdict { get; set; }
    }
}

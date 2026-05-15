namespace Gestao_veiculos.DTOs
{
    public class ResponseProprietarioDto
    {
        public int Id_proprietario { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

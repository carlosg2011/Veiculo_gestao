namespace Gestao_veiculos.DTOs
{
    public class TokenResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracao { get; set; }
    }
}

namespace Gestao_veiculos.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetAsync(string toEmail, string toName, string resetLink);
    }
}

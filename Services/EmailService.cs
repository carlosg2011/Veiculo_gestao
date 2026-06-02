using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Gestao_veiculos.Services
{
    public class EmailService : IEmailService
    {
        private readonly IHttpClientFactory _factory;
        private readonly string _apiKey;
        private readonly string _from;

        public EmailService(IHttpClientFactory factory, IConfiguration config)
        {
            _factory = factory;
            _apiKey = config["Resend:ApiKey"] ?? throw new InvalidOperationException("Resend:ApiKey não configurada.");
            _from = config["Resend:From"] ?? "Vehicle Guard <onboarding@resend.dev>";
        }

        public async Task SendPasswordResetAsync(string toEmail, string toName, string resetLink)
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var payload = new
            {
                from = _from,
                to = new[] { toEmail },
                subject = "Redefinição de senha — Vehicle Guard",
                html = BuildHtml(toName, resetLink)
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("https://api.resend.com/emails", content);
            response.EnsureSuccessStatusCode();
        }

        private static string BuildHtml(string name, string link) => $"""
            <!DOCTYPE html>
            <html lang="pt-BR">
            <body style="font-family:sans-serif;max-width:480px;margin:0 auto;padding:32px;background:#f9fafb">
              <div style="background:#fff;border-radius:12px;padding:32px;border:1px solid #e5e7eb">
                <h2 style="color:#1d4ed8;margin:0 0 8px">Vehicle Guard</h2>
                <p style="color:#374151;margin:0 0 16px">Olá, <strong>{name}</strong>!</p>
                <p style="color:#374151">Recebemos uma solicitação para redefinir a senha da sua conta. Clique no botão abaixo para continuar:</p>
                <div style="text-align:center;margin:28px 0">
                  <a href="{link}"
                     style="display:inline-block;background:#1d4ed8;color:#fff;padding:12px 28px;border-radius:8px;text-decoration:none;font-weight:600;font-size:15px">
                    Redefinir minha senha
                  </a>
                </div>
                <p style="color:#6b7280;font-size:13px;margin:0">
                  Este link expira em <strong>1 hora</strong>. Se você não solicitou a redefinição de senha, ignore este e-mail.
                </p>
              </div>
            </body>
            </html>
            """;
    }
}

using System.Text;
using System.Text.Json;

namespace Gestao_veiculos.Services
{
    public class EmailService : IEmailService
    {
        private readonly IHttpClientFactory _factory;
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IHttpClientFactory factory, IConfiguration config)
        {
            _factory = factory;
            _apiKey = config["Brevo:ApiKey"] ?? throw new InvalidOperationException("Brevo:ApiKey não configurada.");
            _fromEmail = config["Brevo:FromEmail"] ?? throw new InvalidOperationException("Brevo:FromEmail não configurada.");
            _fromName = config["Brevo:FromName"] ?? "Vehicle Guard";
        }

        public async Task SendPasswordResetAsync(string toEmail, string toName, string code)
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("api-key", _apiKey);

            var payload = new
            {
                sender = new { name = _fromName, email = _fromEmail },
                to = new[] { new { email = toEmail, name = toName } },
                subject = "Código de verificação — Vehicle Guard",
                htmlContent = BuildHtml(toName, code)
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("https://api.brevo.com/v3/smtp/email", content);
            response.EnsureSuccessStatusCode();
        }

        private static string BuildHtml(string name, string code) => $"""
            <!DOCTYPE html>
            <html lang="pt-BR">
            <body style="font-family:sans-serif;max-width:480px;margin:0 auto;padding:32px;background:#f9fafb">
              <div style="background:#fff;border-radius:12px;padding:32px;border:1px solid #e5e7eb">
                <h2 style="color:#1d4ed8;margin:0 0 8px">Vehicle Guard</h2>
                <p style="color:#374151;margin:0 0 16px">Olá, <strong>{name}</strong>!</p>
                <p style="color:#374151">Seu código para redefinir a senha é:</p>
                <div style="text-align:center;margin:28px 0">
                  <span style="display:inline-block;background:#f0f4ff;color:#1d4ed8;padding:16px 36px;border-radius:12px;font-size:36px;font-weight:700;letter-spacing:12px;border:2px dashed #bfdbfe">
                    {code}
                  </span>
                </div>
                <p style="color:#374151">Digite esse código no aplicativo para continuar.</p>
                <p style="color:#6b7280;font-size:13px;margin:0">
                  O código expira em <strong>15 minutos</strong>. Se você não solicitou a redefinição de senha, ignore este e-mail.
                </p>
              </div>
            </body>
            </html>
            """;
    }
}

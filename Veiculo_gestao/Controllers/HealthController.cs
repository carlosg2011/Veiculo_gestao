using Microsoft.AspNetCore.Mvc;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() =>
            Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }
}

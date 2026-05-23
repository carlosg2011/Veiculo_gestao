using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? userId)
        {
            int veiculos, propostas, vistorias, termosAssinados, termosPendentes;

            if (userId.HasValue)
            {
                var propostaIds = _context.Propostas
                    .Where(p => p.Id_usuario == userId.Value)
                    .Select(p => p.Id_proposta);

                var veiculoIds = _context.Propostas
                    .Where(p => p.Id_usuario == userId.Value)
                    .Select(p => p.Id_veiculo)
                    .Distinct();

                veiculos         = await veiculoIds.CountAsync();
                propostas        = await _context.Propostas.CountAsync(p => p.Id_usuario == userId.Value);
                vistorias        = await _context.Vistorias.CountAsync(v => v.Id_usuario == userId.Value);
                termosAssinados  = await _context.Termos.CountAsync(t => propostaIds.Contains(t.Id_proposta) && t.DataAssinatura != null);
                termosPendentes  = await _context.Termos.CountAsync(t => propostaIds.Contains(t.Id_proposta) && t.DataAssinatura == null);
            }
            else
            {
                veiculos         = await _context.Veiculos.CountAsync();
                propostas        = await _context.Propostas.CountAsync();
                vistorias        = await _context.Vistorias.CountAsync();
                termosAssinados  = await _context.Termos.CountAsync(t => t.DataAssinatura != null);
                termosPendentes  = await _context.Termos.CountAsync(t => t.DataAssinatura == null);
            }

            return Ok(new DashboardSummaryDto
            {
                Veiculos        = veiculos,
                Propostas       = propostas,
                Vistorias       = vistorias,
                TermosAssinados = termosAssinados,
                TermosPendentes = termosPendentes
            });
        }
    }
}

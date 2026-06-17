using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Enums;
using Gestao_veiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Services
{
    public class TermoService : ITermoService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TermoService> _logger;

        public TermoService(AppDbContext context, ILogger<TermoService> logger)
        {
            _context = context;
            _logger  = logger;
        }

        public async Task<PagedResultDto<ResponseTermoDto>> ListarTodos(PaginationParams pagination, int? userId = null)
        {
            IQueryable<Termo> query;
            if (userId.HasValue)
            {
                var propostaIds = _context.Propostas
                    .Where(p => p.Id_usuario == userId.Value)
                    .Select(p => p.Id_proposta);
                query = _context.Termos.Where(t => propostaIds.Contains(t.Id_proposta));
            }
            else
            {
                query = _context.Termos.AsQueryable();
            }

            var total = await query.CountAsync();
            var items = await query
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(t => ToResponse(t))
                .ToListAsync();

            return new PagedResultDto<ResponseTermoDto>
            {
                Items      = items,
                Page       = pagination.Page,
                PageSize   = pagination.PageSize,
                TotalCount = total
            };
        }

        public async Task<ResponseTermoDto?> BuscarPorId(int id)
        {
            var t = await _context.Termos.FindAsync(id);
            return t is null ? null : ToResponse(t);
        }

        public async Task<ResponseTermoDto?> BuscarPorProposta(int idProposta)
        {
            var t = await _context.Termos
                .Where(x => x.Id_proposta == idProposta && x.Status != StatusTermo.Cancelado && x.Status != StatusTermo.Expirado)
                .OrderByDescending(x => x.DataEnvio)
                .FirstOrDefaultAsync();
            return t is null ? null : ToResponse(t);
        }

        public async Task<ResponseTermoDto> Criar(CreateTermoDto dto)
        {
            if (!await _context.Propostas.AnyAsync(p => p.Id_proposta == dto.Id_proposta))
            {
                _logger.LogWarning("Termo rejeitado — proposta não encontrada: Id={Id}", dto.Id_proposta);
                throw new KeyNotFoundException("Proposta informada não existe.");
            }

            var termo = new Termo
            {
                NumeroTermo    = dto.NumeroTermo,
                Status         = dto.Status!.Value,
                DataEnvio      = dto.DataEnvio,
                DataAssinatura = dto.DataAssinatura,
                Id_proposta    = dto.Id_proposta
            };

            _context.Termos.Add(termo);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Termo criado: Id={Id}, Proposta={IdProposta}", termo.Id_termo, termo.Id_proposta);
            return ToResponse(termo);
        }

        public async Task<ResponseTermoDto> Atualizar(int id, CreateTermoDto dto)
        {
            var termo = await _context.Termos.FindAsync(id);
            if (termo is null)
            {
                _logger.LogWarning("Termo não encontrado para atualização: Id={Id}", id);
                throw new KeyNotFoundException("Termo não encontrado.");
            }

            if (!await _context.Propostas.AnyAsync(p => p.Id_proposta == dto.Id_proposta))
            {
                _logger.LogWarning("Atualização de termo rejeitada — proposta não encontrada: Id={Id}", dto.Id_proposta);
                throw new KeyNotFoundException("Proposta informada não existe.");
            }

            termo.NumeroTermo    = dto.NumeroTermo;
            termo.Status         = dto.Status!.Value;
            termo.DataEnvio      = dto.DataEnvio;
            termo.DataAssinatura = dto.DataAssinatura;
            termo.Id_proposta    = dto.Id_proposta;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Termo atualizado: Id={Id}", id);
            return ToResponse(termo);
        }

        public async Task Deletar(int id)
        {
            var termo = await _context.Termos.FindAsync(id);
            if (termo is null)
            {
                _logger.LogWarning("Termo não encontrado para exclusão: Id={Id}", id);
                throw new KeyNotFoundException("Termo não encontrado.");
            }

            _context.Termos.Remove(termo);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Termo excluído: Id={Id}", id);
        }

        private static ResponseTermoDto ToResponse(Termo t) => new()
        {
            Id_termo       = t.Id_termo,
            NumeroTermo    = t.NumeroTermo,
            Status         = t.Status,
            DataEnvio      = t.DataEnvio,
            DataAssinatura = t.DataAssinatura,
            Id_proposta    = t.Id_proposta
        };
    }
}

using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Services
{
    public class VistoriaService : IVistoriaService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VistoriaService> _logger;

        public VistoriaService(AppDbContext context, ILogger<VistoriaService> logger)
        {
            _context = context;
            _logger  = logger;
        }

        public async Task<PagedResultDto<ResponseVistoriaDto>> ListarTodos(PaginationParams pagination, int? userId = null, int? propostaId = null, bool excludeCancelled = false)
        {
            var query = _context.Vistorias.AsQueryable();
            if (excludeCancelled)
                query = query.Where(v => v.Status != Enums.StatusVistoria.Cancelada);
            if (userId.HasValue)
                query = query.Where(v => v.Id_usuario == userId.Value);
            if (propostaId.HasValue)
                query = query.Where(v => v.Id_proposta == propostaId.Value);

            var total = await query.CountAsync();
            var items = await (from v in query
                               join p   in _context.Propostas      on v.Id_proposta      equals p.Id_proposta
                               join vei in _context.Veiculos        on p.Id_veiculo       equals vei.Id_veiculo
                               join pr  in _context.Proprietarios   on p.Id_proprietario  equals pr.Id_proprietario
                               orderby v.Id_vistoria descending
                               select new ResponseVistoriaDto
                               {
                                   Id_vistoria      = v.Id_vistoria,
                                   DataSolicitacao  = v.DataSolicitacao,
                                   DataInicio       = v.DataInicio,
                                   DataConclusao    = v.DataConclusao,
                                   Status           = v.Status,
                                   Id_proposta      = v.Id_proposta,
                                   SessaoProposta   = p.SessaoProposta,
                                   Id_usuario       = v.Id_usuario,
                                   NomeProprietario = pr.Nome,
                                   Placa            = vei.Placa
                               })
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PagedResultDto<ResponseVistoriaDto>
            {
                Items      = items,
                Page       = pagination.Page,
                PageSize   = pagination.PageSize,
                TotalCount = total
            };
        }

        public async Task<ResponseVistoriaDto?> BuscarPorId(int id)
        {
            return await (from v   in _context.Vistorias
                          where v.Id_vistoria == id
                          join p   in _context.Propostas     on v.Id_proposta     equals p.Id_proposta
                          join vei in _context.Veiculos       on p.Id_veiculo      equals vei.Id_veiculo
                          join pr  in _context.Proprietarios  on p.Id_proprietario equals pr.Id_proprietario
                          select new ResponseVistoriaDto
                          {
                              Id_vistoria      = v.Id_vistoria,
                              DataSolicitacao  = v.DataSolicitacao,
                              DataInicio       = v.DataInicio,
                              DataConclusao    = v.DataConclusao,
                              Status           = v.Status,
                              Id_proposta      = v.Id_proposta,
                              SessaoProposta   = p.SessaoProposta,
                              Id_usuario       = v.Id_usuario,
                              NomeProprietario = pr.Nome,
                              Placa            = vei.Placa
                          }).FirstOrDefaultAsync();
        }

        public async Task<ResponseVistoriaDto> Criar(CreateVistoriaDto dto)
        {
            var proposta = await _context.Propostas.FindAsync(dto.Id_proposta);
            if (proposta is null)
            {
                _logger.LogWarning("Vistoria rejeitada — proposta não encontrada: Id={Id}", dto.Id_proposta);
                throw new KeyNotFoundException("Proposta informada não existe.");
            }
            if (!await _context.Usuarios.AnyAsync(u => u.Id_usuario == dto.Id_usuario))
            {
                _logger.LogWarning("Vistoria rejeitada — usuário não encontrado: Id={Id}", dto.Id_usuario);
                throw new KeyNotFoundException("Usuário informado não existe.");
            }

            var maxId = await _context.Vistorias.MaxAsync(v => (int?)v.Id_vistoria) ?? 9_999;
            int newId = Math.Max(maxId, 9_999) + 1;

            var vistoria = new Vistoria
            {
                Id_vistoria     = newId,
                DataSolicitacao = dto.DataSolicitacao,
                DataInicio      = dto.DataSolicitacao,
                Status          = dto.Status!.Value,
                Id_proposta     = dto.Id_proposta,
                Id_usuario      = dto.Id_usuario
            };

            _context.Vistorias.Add(vistoria);
            proposta.Status = Enums.StatusProposta.EmAnalise;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Vistoria criada: Id={Id}, Proposta={IdProposta} → EmAnalise", vistoria.Id_vistoria, vistoria.Id_proposta);
            return ToResponse(vistoria, proposta.SessaoProposta);
        }

        public async Task<ResponseVistoriaDto> Atualizar(int id, UpdateVistoriaDto dto)
        {
            var vistoria = await _context.Vistorias.FindAsync(id);
            if (vistoria is null)
            {
                _logger.LogWarning("Vistoria não encontrada para atualização: Id={Id}", id);
                throw new KeyNotFoundException("Vistoria não encontrada.");
            }

            vistoria.Status        = dto.Status!.Value;
            vistoria.DataInicio    = dto.DataInicio ?? vistoria.DataInicio;
            vistoria.DataConclusao = dto.DataConclusao;

            await _context.SaveChangesAsync();

            var proposta = await _context.Propostas.FindAsync(vistoria.Id_proposta);
            if (proposta is not null && vistoria.Status == Enums.StatusVistoria.Concluida)
            {
                proposta.Status = Enums.StatusProposta.Aprovada;
                await _context.SaveChangesAsync();
            }

            _logger.LogInformation("Vistoria atualizada: Id={Id}, Status={Status}", id, vistoria.Status);
            return ToResponse(vistoria, proposta?.SessaoProposta ?? string.Empty);
        }

        public async Task Deletar(int id)
        {
            var vistoria = await _context.Vistorias.FindAsync(id);
            if (vistoria is null)
            {
                _logger.LogWarning("Vistoria não encontrada para exclusão: Id={Id}", id);
                throw new KeyNotFoundException("Vistoria não encontrada.");
            }

            _context.Vistorias.Remove(vistoria);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Vistoria excluída: Id={Id}", id);
        }

        private static ResponseVistoriaDto ToResponse(Vistoria v, string sessaoProposta, string nomeProprietario = "", string placa = "") => new()
        {
            Id_vistoria      = v.Id_vistoria,
            DataSolicitacao  = v.DataSolicitacao,
            DataInicio       = v.DataInicio,
            DataConclusao    = v.DataConclusao,
            Status           = v.Status,
            Id_proposta      = v.Id_proposta,
            SessaoProposta   = sessaoProposta,
            Id_usuario       = v.Id_usuario,
            NomeProprietario = nomeProprietario,
            Placa            = placa
        };
    }
}

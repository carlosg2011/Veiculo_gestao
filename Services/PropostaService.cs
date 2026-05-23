using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Enums;
using Gestao_veiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Services
{
    public class PropostaService : IPropostaService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PropostaService> _logger;

        private static readonly StatusProposta[] StatusAtivos =
        [
            StatusProposta.Pendente,
            StatusProposta.EmAnalise,
            StatusProposta.Aprovada
        ];

        public PropostaService(AppDbContext context, ILogger<PropostaService> logger)
        {
            _context = context;
            _logger  = logger;
        }

        public async Task<PagedResultDto<ResponsePropostaDto>> ListarTodos(PaginationParams pagination, int? userId = null)
        {
            var query = userId.HasValue
                ? _context.Propostas.Where(p => p.Id_usuario == userId.Value)
                : _context.Propostas.AsQueryable();

            var total = await query.CountAsync();
            var items = await query
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(p => ToResponse(p))
                .ToListAsync();

            return new PagedResultDto<ResponsePropostaDto>
            {
                Items      = items,
                Page       = pagination.Page,
                PageSize   = pagination.PageSize,
                TotalCount = total
            };
        }

        public async Task<PagedResultDto<ResponsePropostaCompletoDto>> Buscar(PaginationParams pagination, PropostaFiltroParams filtro)
        {
            var query = from p in _context.Propostas
                        join u in _context.Usuarios on p.Id_usuario equals u.Id_usuario
                        join prop in _context.Proprietarios on p.Id_proprietario equals prop.Id_proprietario
                        join v in _context.Veiculos on p.Id_veiculo equals v.Id_veiculo
                        select new { p, u, prop, v };

            if (!string.IsNullOrWhiteSpace(filtro.Nome))
                query = query.Where(x => x.prop.Nome.Contains(filtro.Nome));
            if (!string.IsNullOrWhiteSpace(filtro.Placa))
                query = query.Where(x => x.v.Placa.Contains(filtro.Placa));
            if (!string.IsNullOrWhiteSpace(filtro.Renavam))
                query = query.Where(x => x.v.Renavam.Contains(filtro.Renavam));
            if (!string.IsNullOrWhiteSpace(filtro.Chassi))
                query = query.Where(x => x.v.Chassi.Contains(filtro.Chassi));
            if (filtro.StatusVeiculo.HasValue)
                query = query.Where(x => x.v.Status == filtro.StatusVeiculo.Value);
            if (filtro.StatusProposta.HasValue)
                query = query.Where(x => x.p.Status == filtro.StatusProposta.Value);

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(x => x.p.DataCriacao)
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(x => new ResponsePropostaCompletoDto
                {
                    Id_proposta      = x.p.Id_proposta,
                    SessaoProposta   = x.p.SessaoProposta,
                    DataCriacao      = x.p.DataCriacao,
                    StatusProposta   = x.p.Status,
                    Id_usuario       = x.p.Id_usuario,
                    NomeUsuario      = x.u.Nome,
                    Id_proprietario  = x.prop.Id_proprietario,
                    NomeProprietario = x.prop.Nome,
                    CpfProprietario  = x.prop.Cpf,
                    Id_veiculo       = x.v.Id_veiculo,
                    Placa            = x.v.Placa,
                    Marca            = x.v.Marca,
                    Modelo           = x.v.Modelo,
                    Chassi           = x.v.Chassi,
                    Renavam          = x.v.Renavam,
                    StatusVeiculo    = x.v.Status
                })
                .ToListAsync();

            return new PagedResultDto<ResponsePropostaCompletoDto>
            {
                Items      = items,
                Page       = pagination.Page,
                PageSize   = pagination.PageSize,
                TotalCount = total
            };
        }

        public async Task<ResponsePropostaDto?> BuscarPorId(int id)
        {
            var p = await _context.Propostas.FindAsync(id);
            return p is null ? null : ToResponse(p);
        }

        public async Task<ResponsePropostaDto> Criar(CreatePropostaDto dto)
        {
            if (!await _context.Usuarios.AnyAsync(u => u.Id_usuario == dto.Id_usuario))
            {
                _logger.LogWarning("Proposta rejeitada — usuário não encontrado: Id={Id}", dto.Id_usuario);
                throw new KeyNotFoundException("Usuário informado não existe.");
            }
            if (!await _context.Proprietarios.AnyAsync(p => p.Id_proprietario == dto.Id_proprietario))
            {
                _logger.LogWarning("Proposta rejeitada — proprietário não encontrado: Id={Id}", dto.Id_proprietario);
                throw new KeyNotFoundException("Proprietário informado não existe.");
            }

            var veiculo = await _context.Veiculos.FindAsync(dto.Id_veiculo);
            if (veiculo is null)
            {
                _logger.LogWarning("Proposta rejeitada — veículo não encontrado: Id={Id}", dto.Id_veiculo);
                throw new KeyNotFoundException("Veículo informado não existe.");
            }

            if (veiculo.Status == StatusVeiculo.Bloqueado)
            {
                _logger.LogWarning("Proposta rejeitada — veículo bloqueado: Id={Id}, Placa={Placa}", veiculo.Id_veiculo, veiculo.Placa);
                throw new InvalidOperationException("Veículo indisponível por inviabilidade técnica.");
            }

            if (await _context.Propostas.AnyAsync(p =>
                    p.Id_veiculo == dto.Id_veiculo && StatusAtivos.Contains(p.Status)))
            {
                _logger.LogWarning("Proposta rejeitada — proposta ativa existente para a placa {Placa}", veiculo.Placa);
                throw new InvalidOperationException($"Contrato emitido para a placa {veiculo.Placa}.");
            }

            if (await _context.Propostas.AnyAsync(p => p.SessaoProposta == dto.SessaoProposta))
            {
                _logger.LogWarning("Proposta rejeitada — código duplicado: {Sessao}", dto.SessaoProposta);
                throw new InvalidOperationException("Já existe uma proposta com esse código.");
            }

            var proposta = new Proposta
            {
                SessaoProposta  = dto.SessaoProposta,
                Status          = dto.Status!.Value,
                DataCriacao     = DateTime.UtcNow,
                Id_usuario      = dto.Id_usuario,
                Id_veiculo      = dto.Id_veiculo,
                Id_proprietario = dto.Id_proprietario
            };

            _context.Propostas.Add(proposta);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Proposta criada: Id={Id}, Placa={Placa}", proposta.Id_proposta, veiculo.Placa);
            return ToResponse(proposta);
        }

        public async Task<ResponsePropostaDto> Atualizar(int id, CreatePropostaDto dto)
        {
            var proposta = await _context.Propostas.FindAsync(id);
            if (proposta is null)
            {
                _logger.LogWarning("Proposta não encontrada para atualização: Id={Id}", id);
                throw new KeyNotFoundException("Proposta não encontrada.");
            }

            if (!await _context.Usuarios.AnyAsync(u => u.Id_usuario == dto.Id_usuario))
            {
                _logger.LogWarning("Atualização de proposta rejeitada — usuário não encontrado: Id={Id}", dto.Id_usuario);
                throw new KeyNotFoundException("Usuário informado não existe.");
            }
            if (!await _context.Proprietarios.AnyAsync(p => p.Id_proprietario == dto.Id_proprietario))
            {
                _logger.LogWarning("Atualização de proposta rejeitada — proprietário não encontrado: Id={Id}", dto.Id_proprietario);
                throw new KeyNotFoundException("Proprietário informado não existe.");
            }

            var veiculo = await _context.Veiculos.FindAsync(dto.Id_veiculo);
            if (veiculo is null)
            {
                _logger.LogWarning("Atualização de proposta rejeitada — veículo não encontrado: Id={Id}", dto.Id_veiculo);
                throw new KeyNotFoundException("Veículo informado não existe.");
            }

            if (veiculo.Status == StatusVeiculo.Bloqueado)
            {
                _logger.LogWarning("Atualização de proposta rejeitada — veículo bloqueado: Placa={Placa}", veiculo.Placa);
                throw new InvalidOperationException("Veículo indisponível por inviabilidade técnica.");
            }

            if (await _context.Propostas.AnyAsync(p =>
                    p.Id_veiculo == dto.Id_veiculo && p.Id_proposta != id && StatusAtivos.Contains(p.Status)))
            {
                _logger.LogWarning("Atualização de proposta rejeitada — proposta ativa existente para a placa {Placa}", veiculo.Placa);
                throw new InvalidOperationException($"Contrato emitido para a placa {veiculo.Placa}.");
            }

            if (await _context.Propostas.AnyAsync(p => p.SessaoProposta == dto.SessaoProposta && p.Id_proposta != id))
            {
                _logger.LogWarning("Atualização de proposta rejeitada — código duplicado: {Sessao}", dto.SessaoProposta);
                throw new InvalidOperationException("Já existe outra proposta com esse código.");
            }

            proposta.SessaoProposta  = dto.SessaoProposta;
            proposta.Status          = dto.Status!.Value;
            proposta.Id_usuario      = dto.Id_usuario;
            proposta.Id_veiculo      = dto.Id_veiculo;
            proposta.Id_proprietario = dto.Id_proprietario;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Proposta atualizada: Id={Id}", id);
            return ToResponse(proposta);
        }

        public async Task Deletar(int id)
        {
            var proposta = await _context.Propostas.FindAsync(id);
            if (proposta is null)
            {
                _logger.LogWarning("Proposta não encontrada para exclusão: Id={Id}", id);
                throw new KeyNotFoundException("Proposta não encontrada.");
            }

            _context.Propostas.Remove(proposta);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Proposta excluída: Id={Id}", id);
        }

        private static ResponsePropostaDto ToResponse(Proposta p) => new()
        {
            Id_proposta     = p.Id_proposta,
            SessaoProposta  = p.SessaoProposta,
            DataCriacao     = p.DataCriacao,
            Status          = p.Status,
            Id_usuario      = p.Id_usuario,
            Id_veiculo      = p.Id_veiculo,
            Id_proprietario = p.Id_proprietario
        };
    }
}

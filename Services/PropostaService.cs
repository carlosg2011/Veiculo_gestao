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

        private static readonly StatusProposta[] StatusAtivos =
        [
            StatusProposta.Pendente,
            StatusProposta.EmAnalise,
            StatusProposta.Aprovada
        ];

        public PropostaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultDto<ResponsePropostaDto>> ListarTodos(PaginationParams pagination)
        {
            var total = await _context.Propostas.CountAsync();
            var items = await _context.Propostas
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

        public async Task<ResponsePropostaDto?> BuscarPorId(int id)
        {
            var p = await _context.Propostas.FindAsync(id);
            return p is null ? null : ToResponse(p);
        }

        public async Task<ResponsePropostaDto> Criar(CreatePropostaDto dto)
        {
            if (!await _context.Usuarios.AnyAsync(u => u.Id_usuario == dto.Id_usuario))
                throw new KeyNotFoundException("Usuário informado não existe.");
            if (!await _context.Proprietarios.AnyAsync(p => p.Id_proprietario == dto.Id_proprietario))
                throw new KeyNotFoundException("Proprietário informado não existe.");

            var veiculo = await _context.Veiculos.FindAsync(dto.Id_veiculo)
                ?? throw new KeyNotFoundException("Veículo informado não existe.");

            if (veiculo.Status == StatusVeiculo.Bloqueado)
                throw new InvalidOperationException("Veículo indisponível por inviabilidade técnica.");

            if (await _context.Propostas.AnyAsync(p =>
                    p.Id_veiculo == dto.Id_veiculo && StatusAtivos.Contains(p.Status)))
                throw new InvalidOperationException($"Contrato emitido para a placa {veiculo.Placa}.");

            if (await _context.Propostas.AnyAsync(p => p.sessao_proposta == dto.Sessao_proposta))
                throw new InvalidOperationException("Já existe uma proposta com esse código.");

            var proposta = new Proposta
            {
                sessao_proposta = dto.Sessao_proposta,
                Status          = dto.Status!.Value,
                Data_Criacao    = DateTime.UtcNow,
                Id_usuario      = dto.Id_usuario,
                Id_veiculo      = dto.Id_veiculo,
                Id_proprietario = dto.Id_proprietario
            };

            _context.Propostas.Add(proposta);
            await _context.SaveChangesAsync();

            return ToResponse(proposta);
        }

        public async Task<ResponsePropostaDto> Atualizar(int id, CreatePropostaDto dto)
        {
            var proposta = await _context.Propostas.FindAsync(id)
                ?? throw new KeyNotFoundException("Proposta não encontrada.");

            if (!await _context.Usuarios.AnyAsync(u => u.Id_usuario == dto.Id_usuario))
                throw new KeyNotFoundException("Usuário informado não existe.");
            if (!await _context.Proprietarios.AnyAsync(p => p.Id_proprietario == dto.Id_proprietario))
                throw new KeyNotFoundException("Proprietário informado não existe.");

            var veiculo = await _context.Veiculos.FindAsync(dto.Id_veiculo)
                ?? throw new KeyNotFoundException("Veículo informado não existe.");

            if (veiculo.Status == StatusVeiculo.Bloqueado)
                throw new InvalidOperationException("Veículo indisponível por inviabilidade técnica.");

            if (await _context.Propostas.AnyAsync(p =>
                    p.Id_veiculo == dto.Id_veiculo && p.Id_proposta != id && StatusAtivos.Contains(p.Status)))
                throw new InvalidOperationException($"Contrato emitido para a placa {veiculo.Placa}.");

            if (await _context.Propostas.AnyAsync(p => p.sessao_proposta == dto.Sessao_proposta && p.Id_proposta != id))
                throw new InvalidOperationException("Já existe outra proposta com esse código.");

            proposta.sessao_proposta = dto.Sessao_proposta;
            proposta.Status          = dto.Status!.Value;
            proposta.Id_usuario      = dto.Id_usuario;
            proposta.Id_veiculo      = dto.Id_veiculo;
            proposta.Id_proprietario = dto.Id_proprietario;

            await _context.SaveChangesAsync();

            return ToResponse(proposta);
        }

        public async Task Deletar(int id)
        {
            var proposta = await _context.Propostas.FindAsync(id)
                ?? throw new KeyNotFoundException("Proposta não encontrada.");

            _context.Propostas.Remove(proposta);
            await _context.SaveChangesAsync();
        }

        private static ResponsePropostaDto ToResponse(Proposta p) => new()
        {
            Id_proposta     = p.Id_proposta,
            Sessao_proposta = p.sessao_proposta,
            Data_Criacao    = p.Data_Criacao,
            Status          = p.Status,
            Id_usuario      = p.Id_usuario,
            Id_veiculo      = p.Id_veiculo,
            Id_proprietario = p.Id_proprietario
        };
    }
}

using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Enums;
using Gestao_veiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VeiculoService> _logger;

        public VeiculoService(AppDbContext context, ILogger<VeiculoService> logger)
        {
            _context = context;
            _logger  = logger;
        }

        public async Task<PagedResultDto<ResponseVeiculoDto>> ListarTodos(PaginationParams pagination)
        {
            var total = await _context.Veiculos.CountAsync();
            var items = await _context.Veiculos
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(v => ToResponse(v))
                .ToListAsync();

            return new PagedResultDto<ResponseVeiculoDto>
            {
                Items      = items,
                Page       = pagination.Page,
                PageSize   = pagination.PageSize,
                TotalCount = total
            };
        }

        public async Task<ResponseVeiculoDto?> BuscarPorId(int id)
        {
            var v = await _context.Veiculos.FindAsync(id);
            return v is null ? null : ToResponse(v);
        }

        public async Task<ResponseVeiculoDto> Criar(CreateVeiculoDto dto)
        {
            var veiculoExistente = await _context.Veiculos.FirstOrDefaultAsync(v => v.Placa == dto.Placa);
            if (veiculoExistente is not null)
            {
                if (veiculoExistente.Status == StatusVeiculo.Bloqueado)
                {
                    _logger.LogWarning("Tentativa de cadastro com placa bloqueada: {Placa}", dto.Placa);
                    throw new InvalidOperationException("Veículo indisponível por inviabilidade técnica.");
                }
                _logger.LogWarning("Tentativa de cadastro com placa já existente: {Placa}", dto.Placa);
                throw new InvalidOperationException("Placa já cadastrada.");
            }
            if (await _context.Veiculos.AnyAsync(v => v.Chassi == dto.Chassi))
            {
                _logger.LogWarning("Tentativa de cadastro com chassi já existente: {Chassi}", dto.Chassi);
                throw new InvalidOperationException("Chassi já cadastrado.");
            }
            if (await _context.Veiculos.AnyAsync(v => v.Renavam == dto.Renavam))
            {
                _logger.LogWarning("Tentativa de cadastro com RENAVAM já existente: {Renavam}", dto.Renavam);
                throw new InvalidOperationException("Renavam já cadastrado.");
            }

            var veiculo = new Veiculo
            {
                Placa   = dto.Placa,
                Marca   = dto.Marca,
                Modelo  = dto.Modelo,
                AnoFab  = dto.AnoFab,
                AnoMod  = dto.AnoMod,
                Chassi  = dto.Chassi,
                Renavam = dto.Renavam,
                Cor     = dto.Cor,
                Status  = dto.Status!.Value
            };

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Veículo criado: Id={Id}, Placa={Placa}", veiculo.Id_veiculo, veiculo.Placa);
            return ToResponse(veiculo);
        }

        public async Task<ResponseVeiculoDto> Atualizar(int id, CreateVeiculoDto dto)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo is null)
            {
                _logger.LogWarning("Veículo não encontrado para atualização: Id={Id}", id);
                throw new KeyNotFoundException("Veículo não encontrado.");
            }

            if (await _context.Veiculos.AnyAsync(v => v.Placa == dto.Placa && v.Id_veiculo != id))
            {
                _logger.LogWarning("Placa em conflito na atualização do veículo Id={Id}: {Placa}", id, dto.Placa);
                throw new InvalidOperationException("Placa já está em uso por outro veículo.");
            }
            if (await _context.Veiculos.AnyAsync(v => v.Chassi == dto.Chassi && v.Id_veiculo != id))
            {
                _logger.LogWarning("Chassi em conflito na atualização do veículo Id={Id}: {Chassi}", id, dto.Chassi);
                throw new InvalidOperationException("Chassi já está em uso por outro veículo.");
            }
            if (await _context.Veiculos.AnyAsync(v => v.Renavam == dto.Renavam && v.Id_veiculo != id))
            {
                _logger.LogWarning("RENAVAM em conflito na atualização do veículo Id={Id}: {Renavam}", id, dto.Renavam);
                throw new InvalidOperationException("Renavam já está em uso por outro veículo.");
            }

            veiculo.Placa   = dto.Placa;
            veiculo.Marca   = dto.Marca;
            veiculo.Modelo  = dto.Modelo;
            veiculo.AnoFab  = dto.AnoFab;
            veiculo.AnoMod  = dto.AnoMod;
            veiculo.Chassi  = dto.Chassi;
            veiculo.Renavam = dto.Renavam;
            veiculo.Cor     = dto.Cor;
            veiculo.Status  = dto.Status!.Value;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Veículo atualizado: Id={Id}, Placa={Placa}", id, veiculo.Placa);
            return ToResponse(veiculo);
        }

        public async Task Deletar(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo is null)
            {
                _logger.LogWarning("Veículo não encontrado para exclusão: Id={Id}", id);
                throw new KeyNotFoundException("Veículo não encontrado.");
            }

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Veículo excluído: Id={Id}, Placa={Placa}", id, veiculo.Placa);
        }

        private static ResponseVeiculoDto ToResponse(Veiculo v) => new()
        {
            Id_veiculo = v.Id_veiculo,
            Placa      = v.Placa,
            Marca      = v.Marca,
            Modelo     = v.Modelo,
            AnoFab     = v.AnoFab,
            AnoMod     = v.AnoMod,
            Chassi     = v.Chassi,
            Renavam    = v.Renavam,
            Cor        = v.Cor,
            Status     = v.Status
        };
    }
}

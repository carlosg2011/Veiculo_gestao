using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly AppDbContext _context;

        public VeiculoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResponseVeiculoDto>> ListarTodos() =>
            await _context.Veiculos.Select(v => ToResponse(v)).ToListAsync();

        public async Task<ResponseVeiculoDto?> BuscarPorId(int id)
        {
            var v = await _context.Veiculos.FindAsync(id);
            return v is null ? null : ToResponse(v);
        }

        public async Task<ResponseVeiculoDto> Criar(CreateVeiculoDto dto)
        {
            if (await _context.Veiculos.AnyAsync(v => v.Placa == dto.Placa))
                throw new InvalidOperationException("Placa já cadastrada.");
            if (await _context.Veiculos.AnyAsync(v => v.Chassi == dto.Chassi))
                throw new InvalidOperationException("Chassi já cadastrado.");
            if (await _context.Veiculos.AnyAsync(v => v.Renavam == dto.Renavam))
                throw new InvalidOperationException("Renavam já cadastrado.");

            var veiculo = new Veiculo
            {
                Placa   = dto.Placa,
                Marca   = dto.Marca,
                Modelo  = dto.Modelo,
                Ano_Fab = dto.Ano_Fab,
                Ano_Mod = dto.Ano_Mod,
                Chassi  = dto.Chassi,
                Renavam = dto.Renavam,
                Cor     = dto.Cor,
                Status  = dto.Status!.Value
            };

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return ToResponse(veiculo);
        }

        public async Task<ResponseVeiculoDto> Atualizar(int id, CreateVeiculoDto dto)
        {
            var veiculo = await _context.Veiculos.FindAsync(id)
                ?? throw new KeyNotFoundException("Veículo não encontrado.");

            if (await _context.Veiculos.AnyAsync(v => v.Placa == dto.Placa && v.Id_veiculo != id))
                throw new InvalidOperationException("Placa já está em uso por outro veículo.");
            if (await _context.Veiculos.AnyAsync(v => v.Chassi == dto.Chassi && v.Id_veiculo != id))
                throw new InvalidOperationException("Chassi já está em uso por outro veículo.");
            if (await _context.Veiculos.AnyAsync(v => v.Renavam == dto.Renavam && v.Id_veiculo != id))
                throw new InvalidOperationException("Renavam já está em uso por outro veículo.");

            veiculo.Placa   = dto.Placa;
            veiculo.Marca   = dto.Marca;
            veiculo.Modelo  = dto.Modelo;
            veiculo.Ano_Fab = dto.Ano_Fab;
            veiculo.Ano_Mod = dto.Ano_Mod;
            veiculo.Chassi  = dto.Chassi;
            veiculo.Renavam = dto.Renavam;
            veiculo.Cor     = dto.Cor;
            veiculo.Status  = dto.Status!.Value;

            await _context.SaveChangesAsync();

            return ToResponse(veiculo);
        }

        public async Task Deletar(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id)
                ?? throw new KeyNotFoundException("Veículo não encontrado.");

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();
        }

        private static ResponseVeiculoDto ToResponse(Veiculo v) => new()
        {
            Id_veiculo = v.Id_veiculo,
            Placa      = v.Placa,
            Marca      = v.Marca,
            Modelo     = v.Modelo,
            Ano_Fab    = v.Ano_Fab,
            Ano_Mod    = v.Ano_Mod,
            Chassi     = v.Chassi,
            Renavam    = v.Renavam,
            Cor        = v.Cor,
            Status     = v.Status
        };
    }
}

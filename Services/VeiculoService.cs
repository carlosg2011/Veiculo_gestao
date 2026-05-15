using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Models;

namespace Gestao_veiculos.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly AppDbContext _context;

        public VeiculoService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ResponseVeiculoDto> ListarTodos() =>
            _context.Veiculos.Select(v => ToResponse(v)).ToList();

        public ResponseVeiculoDto? BuscarPorId(int id)
        {
            var v = _context.Veiculos.Find(id);
            return v is null ? null : ToResponse(v);
        }

        public ResponseVeiculoDto Criar(CreateVeiculoDto dto)
        {
            if (_context.Veiculos.Any(v => v.Placa == dto.Placa))
                throw new InvalidOperationException("Placa já cadastrada.");
            if (_context.Veiculos.Any(v => v.Chassi == dto.Chassi))
                throw new InvalidOperationException("Chassi já cadastrado.");
            if (_context.Veiculos.Any(v => v.Renavam == dto.Renavam))
                throw new InvalidOperationException("Renavam já cadastrado.");

            var veiculo = new Veiculo
            {
                Placa = dto.Placa,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Ano_Fab = dto.Ano_Fab,
                Ano_Mod = dto.Ano_Mod,
                Chassi = dto.Chassi,
                Renavam = dto.Renavam,
                Cor = dto.Cor,
                Status = dto.Status
            };

            _context.Veiculos.Add(veiculo);
            _context.SaveChanges();

            return ToResponse(veiculo);
        }

        public ResponseVeiculoDto Atualizar(int id, CreateVeiculoDto dto)
        {
            var veiculo = _context.Veiculos.Find(id)
                ?? throw new KeyNotFoundException("Veículo não encontrado.");

            if (_context.Veiculos.Any(v => v.Placa == dto.Placa && v.Id_veiculo != id))
                throw new InvalidOperationException("Placa já está em uso por outro veículo.");
            if (_context.Veiculos.Any(v => v.Chassi == dto.Chassi && v.Id_veiculo != id))
                throw new InvalidOperationException("Chassi já está em uso por outro veículo.");
            if (_context.Veiculos.Any(v => v.Renavam == dto.Renavam && v.Id_veiculo != id))
                throw new InvalidOperationException("Renavam já está em uso por outro veículo.");

            veiculo.Placa = dto.Placa;
            veiculo.Marca = dto.Marca;
            veiculo.Modelo = dto.Modelo;
            veiculo.Ano_Fab = dto.Ano_Fab;
            veiculo.Ano_Mod = dto.Ano_Mod;
            veiculo.Chassi = dto.Chassi;
            veiculo.Renavam = dto.Renavam;
            veiculo.Cor = dto.Cor;
            veiculo.Status = dto.Status;

            _context.SaveChanges();

            return ToResponse(veiculo);
        }

        public void Deletar(int id)
        {
            var veiculo = _context.Veiculos.Find(id)
                ?? throw new KeyNotFoundException("Veículo não encontrado.");

            _context.Veiculos.Remove(veiculo);
            _context.SaveChanges();
        }

        private static ResponseVeiculoDto ToResponse(Veiculo v) => new()
        {
            Id_veiculo = v.Id_veiculo,
            Placa = v.Placa,
            Marca = v.Marca,
            Modelo = v.Modelo,
            Ano_Fab = v.Ano_Fab,
            Ano_Mod = v.Ano_Mod,
            Chassi = v.Chassi,
            Renavam = v.Renavam,
            Cor = v.Cor,
            Status = v.Status
        };
    }
}

using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Models;

namespace Gestao_veiculos.Services
{
    public class ProprietarioService : IProprietarioService
    {
        private readonly AppDbContext _context;

        public ProprietarioService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ResponseProprietarioDto> ListarTodos() =>
            _context.Proprietarios.Select(p => ToResponse(p)).ToList();

        public ResponseProprietarioDto? BuscarPorId(int id)
        {
            var p = _context.Proprietarios.Find(id);
            return p is null ? null : ToResponse(p);
        }

        public ResponseProprietarioDto Criar(CreateProprietarioDto dto)
        {
            if (_context.Proprietarios.Any(p => p.Cpf == dto.Cpf))
                throw new InvalidOperationException("CPF já cadastrado.");

            var proprietario = new Proprietario
            {
                Nome = dto.Nome,
                Cpf = dto.Cpf,
                Telefone = dto.Telefone,
                Email = dto.Email
            };

            _context.Proprietarios.Add(proprietario);
            _context.SaveChanges();

            return ToResponse(proprietario);
        }

        public ResponseProprietarioDto Atualizar(int id, CreateProprietarioDto dto)
        {
            var proprietario = _context.Proprietarios.Find(id)
                ?? throw new KeyNotFoundException("Proprietário não encontrado.");

            if (_context.Proprietarios.Any(p => p.Cpf == dto.Cpf && p.Id_proprietario != id))
                throw new InvalidOperationException("CPF já está em uso por outro proprietário.");

            proprietario.Nome = dto.Nome;
            proprietario.Cpf = dto.Cpf;
            proprietario.Telefone = dto.Telefone;
            proprietario.Email = dto.Email;

            _context.SaveChanges();

            return ToResponse(proprietario);
        }

        public void Deletar(int id)
        {
            var proprietario = _context.Proprietarios.Find(id)
                ?? throw new KeyNotFoundException("Proprietário não encontrado.");

            _context.Proprietarios.Remove(proprietario);
            _context.SaveChanges();
        }

        private static ResponseProprietarioDto ToResponse(Proprietario p) => new()
        {
            Id_proprietario = p.Id_proprietario,
            Nome = p.Nome,
            Cpf = p.Cpf,
            Telefone = p.Telefone,
            Email = p.Email
        };
    }
}

using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Models;

namespace Gestao_veiculos.Services
{
    public class PropostaService : IPropostaService
    {
        private readonly AppDbContext _context;

        public PropostaService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ResponsePropostaDto> ListarTodos() =>
            _context.Propostas.Select(p => ToResponse(p)).ToList();

        public ResponsePropostaDto? BuscarPorId(int id)
        {
            var p = _context.Propostas.Find(id);
            return p is null ? null : ToResponse(p);
        }

        public ResponsePropostaDto Criar(CreatePropostaDto dto)
        {
            if (!_context.Usuarios.Any(u => u.Id_usuario == dto.Id_usuario))
                throw new KeyNotFoundException("Usuário informado não existe.");
            if (!_context.Proprietarios.Any(p => p.Id_proprietario == dto.Id_proprietario))
                throw new KeyNotFoundException("Proprietário informado não existe.");
            if (!_context.Veiculos.Any(v => v.Id_veiculo == dto.Id_veiculo))
                throw new KeyNotFoundException("Veículo informado não existe.");
            if (_context.Propostas.Any(p => p.sessao_proposta == dto.Sessao_proposta))
                throw new InvalidOperationException("Já existe uma proposta com esse código.");

            var proposta = new Proposta
            {
                sessao_proposta = dto.Sessao_proposta,
                Status = dto.Status,
                Data_Criacao = DateTime.UtcNow,
                Id_usuario = dto.Id_usuario,
                Id_veiculo = dto.Id_veiculo,
                Id_proprietario = dto.Id_proprietario
            };

            _context.Propostas.Add(proposta);
            _context.SaveChanges();

            return ToResponse(proposta);
        }

        public ResponsePropostaDto Atualizar(int id, CreatePropostaDto dto)
        {
            var proposta = _context.Propostas.Find(id)
                ?? throw new KeyNotFoundException("Proposta não encontrada.");

            if (!_context.Usuarios.Any(u => u.Id_usuario == dto.Id_usuario))
                throw new KeyNotFoundException("Usuário informado não existe.");
            if (!_context.Proprietarios.Any(p => p.Id_proprietario == dto.Id_proprietario))
                throw new KeyNotFoundException("Proprietário informado não existe.");
            if (!_context.Veiculos.Any(v => v.Id_veiculo == dto.Id_veiculo))
                throw new KeyNotFoundException("Veículo informado não existe.");
            if (_context.Propostas.Any(p => p.sessao_proposta == dto.Sessao_proposta && p.Id_proposta != id))
                throw new InvalidOperationException("Já existe outra proposta com esse código.");

            proposta.sessao_proposta = dto.Sessao_proposta;
            proposta.Status = dto.Status;
            proposta.Id_usuario = dto.Id_usuario;
            proposta.Id_veiculo = dto.Id_veiculo;
            proposta.Id_proprietario = dto.Id_proprietario;

            _context.SaveChanges();

            return ToResponse(proposta);
        }

        public void Deletar(int id)
        {
            var proposta = _context.Propostas.Find(id)
                ?? throw new KeyNotFoundException("Proposta não encontrada.");

            _context.Propostas.Remove(proposta);
            _context.SaveChanges();
        }

        private static ResponsePropostaDto ToResponse(Proposta p) => new()
        {
            Id_proposta = p.Id_proposta,
            Sessao_proposta = p.sessao_proposta,
            Data_Criacao = p.Data_Criacao,
            Status = p.Status,
            Id_usuario = p.Id_usuario,
            Id_veiculo = p.Id_veiculo,
            Id_proprietario = p.Id_proprietario
        };
    }
}

using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Models;

namespace Gestao_veiculos.Services
{
    public class TermoService : ITermoService
    {
        private readonly AppDbContext _context;

        public TermoService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ResponseTermoDto> ListarTodos() =>
            _context.Termos.Select(t => ToResponse(t)).ToList();

        public ResponseTermoDto? BuscarPorId(int id)
        {
            var t = _context.Termos.Find(id);
            return t is null ? null : ToResponse(t);
        }

        public ResponseTermoDto Criar(CreateTermoDto dto)
        {
            if (!_context.Propostas.Any(p => p.Id_proposta == dto.Id_proposta))
                throw new KeyNotFoundException("Proposta informada não existe.");

            var termo = new Termo
            {
                numero_termo = dto.Numero_termo,
                status_termo = dto.Status_termo,
                data_envio = dto.Data_envio,
                data_assinatura = dto.Data_assinatura,
                Id_proposta = dto.Id_proposta
            };

            _context.Termos.Add(termo);
            _context.SaveChanges();

            return ToResponse(termo);
        }

        public ResponseTermoDto Atualizar(int id, CreateTermoDto dto)
        {
            var termo = _context.Termos.Find(id)
                ?? throw new KeyNotFoundException("Termo não encontrado.");

            if (!_context.Propostas.Any(p => p.Id_proposta == dto.Id_proposta))
                throw new KeyNotFoundException("Proposta informada não existe.");

            termo.numero_termo = dto.Numero_termo;
            termo.status_termo = dto.Status_termo;
            termo.data_envio = dto.Data_envio;
            termo.data_assinatura = dto.Data_assinatura;
            termo.Id_proposta = dto.Id_proposta;

            _context.SaveChanges();

            return ToResponse(termo);
        }

        public void Deletar(int id)
        {
            var termo = _context.Termos.Find(id)
                ?? throw new KeyNotFoundException("Termo não encontrado.");

            _context.Termos.Remove(termo);
            _context.SaveChanges();
        }

        private static ResponseTermoDto ToResponse(Termo t) => new()
        {
            Id_termo = t.Id_termo,
            Numero_termo = t.numero_termo,
            Status_termo = t.status_termo,
            Data_envio = t.data_envio,
            Data_assinatura = t.data_assinatura,
            Id_proposta = t.Id_proposta
        };
    }
}

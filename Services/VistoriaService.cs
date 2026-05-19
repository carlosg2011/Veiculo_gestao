using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Services
{
    public class VistoriaService : IVistoriaService
    {
        private readonly AppDbContext _context;

        public VistoriaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResponseVistoriaDto>> ListarTodos() =>
            await _context.Vistorias.Select(v => ToResponse(v)).ToListAsync();

        public async Task<ResponseVistoriaDto?> BuscarPorId(int id)
        {
            var v = await _context.Vistorias.FindAsync(id);
            return v is null ? null : ToResponse(v);
        }

        public async Task<ResponseVistoriaDto> Criar(CreateVistoriaDto dto)
        {
            if (!await _context.Propostas.AnyAsync(p => p.Id_proposta == dto.Id_proposta))
                throw new KeyNotFoundException("Proposta informada não existe.");
            if (!await _context.Usuarios.AnyAsync(u => u.Id_usuario == dto.Id_usuario))
                throw new KeyNotFoundException("Usuário informado não existe.");

            var vistoria = new Vistoria
            {
                Data_solicitacao = dto.Data_solicitacao,
                status_vistoria  = dto.Status_vistoria,
                Id_proposta      = dto.Id_proposta,
                Id_usuario       = dto.Id_usuario
            };

            _context.Vistorias.Add(vistoria);
            await _context.SaveChangesAsync();

            return ToResponse(vistoria);
        }

        public async Task<ResponseVistoriaDto> Atualizar(int id, UpdateVistoriaDto dto)
        {
            var vistoria = await _context.Vistorias.FindAsync(id)
                ?? throw new KeyNotFoundException("Vistoria não encontrada.");

            vistoria.status_vistoria = dto.Status_vistoria;
            vistoria.data_inicio     = dto.Data_inicio;
            vistoria.data_conclusao  = dto.Data_conclusao;

            await _context.SaveChangesAsync();

            return ToResponse(vistoria);
        }

        public async Task Deletar(int id)
        {
            var vistoria = await _context.Vistorias.FindAsync(id)
                ?? throw new KeyNotFoundException("Vistoria não encontrada.");

            _context.Vistorias.Remove(vistoria);
            await _context.SaveChangesAsync();
        }

        private static ResponseVistoriaDto ToResponse(Vistoria v) => new()
        {
            Id_vistoria      = v.Id_vistoria,
            Data_solicitacao = v.Data_solicitacao,
            Data_inicio      = v.data_inicio,
            Data_conclusao   = v.data_conclusao,
            Status_vistoria  = v.status_vistoria,
            Id_proposta      = v.Id_proposta,
            Id_usuario       = v.Id_usuario
        };
    }
}

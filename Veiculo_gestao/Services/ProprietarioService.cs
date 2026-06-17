using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Services
{
    public class ProprietarioService : IProprietarioService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProprietarioService> _logger;

        public ProprietarioService(AppDbContext context, ILogger<ProprietarioService> logger)
        {
            _context = context;
            _logger  = logger;
        }

        public async Task<PagedResultDto<ResponseProprietarioDto>> ListarTodos(PaginationParams pagination)
        {
            var total = await _context.Proprietarios.CountAsync();
            var items = await _context.Proprietarios
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(p => ToResponse(p))
                .ToListAsync();

            return new PagedResultDto<ResponseProprietarioDto>
            {
                Items      = items,
                Page       = pagination.Page,
                PageSize   = pagination.PageSize,
                TotalCount = total
            };
        }

        public async Task<ResponseProprietarioDto?> BuscarPorId(int id)
        {
            var p = await _context.Proprietarios.FindAsync(id);
            return p is null ? null : ToResponse(p);
        }

        public async Task<ResponseProprietarioDto?> BuscarPorCpf(string cpf)
        {
            var p = await _context.Proprietarios.FirstOrDefaultAsync(x => x.Cpf == cpf);
            return p is null ? null : ToResponse(p);
        }

        public async Task<ResponseProprietarioDto> Criar(CreateProprietarioDto dto)
        {
            if (await _context.Proprietarios.AnyAsync(p => p.Cpf == dto.Cpf))
            {
                _logger.LogWarning("Tentativa de cadastro com CPF já existente: {Cpf}", dto.Cpf);
                throw new InvalidOperationException("CPF já cadastrado.");
            }

            var proprietario = new Proprietario
            {
                Nome     = dto.Nome,
                Cpf      = dto.Cpf,
                Telefone = dto.Telefone,
                Email    = dto.Email
            };

            _context.Proprietarios.Add(proprietario);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Proprietário criado: Id={Id}, CPF={Cpf}", proprietario.Id_proprietario, proprietario.Cpf);
            return ToResponse(proprietario);
        }

        public async Task<ResponseProprietarioDto> Atualizar(int id, CreateProprietarioDto dto)
        {
            var proprietario = await _context.Proprietarios.FindAsync(id);
            if (proprietario is null)
            {
                _logger.LogWarning("Proprietário não encontrado para atualização: Id={Id}", id);
                throw new KeyNotFoundException("Proprietário não encontrado.");
            }

            if (await _context.Proprietarios.AnyAsync(p => p.Cpf == dto.Cpf && p.Id_proprietario != id))
            {
                _logger.LogWarning("CPF em conflito na atualização do proprietário Id={Id}: {Cpf}", id, dto.Cpf);
                throw new InvalidOperationException("CPF já está em uso por outro proprietário.");
            }

            proprietario.Nome     = dto.Nome;
            proprietario.Cpf      = dto.Cpf;
            proprietario.Telefone = dto.Telefone;
            proprietario.Email    = dto.Email;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Proprietário atualizado: Id={Id}", id);
            return ToResponse(proprietario);
        }

        public async Task Deletar(int id)
        {
            var proprietario = await _context.Proprietarios.FindAsync(id);
            if (proprietario is null)
            {
                _logger.LogWarning("Proprietário não encontrado para exclusão: Id={Id}", id);
                throw new KeyNotFoundException("Proprietário não encontrado.");
            }

            _context.Proprietarios.Remove(proprietario);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Proprietário excluído: Id={Id}", id);
        }

        private static ResponseProprietarioDto ToResponse(Proprietario p) => new()
        {
            Id_proprietario = p.Id_proprietario,
            Nome            = p.Nome,
            Cpf             = p.Cpf,
            Telefone        = p.Telefone,
            Email           = p.Email
        };
    }
}

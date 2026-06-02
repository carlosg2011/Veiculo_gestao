using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(AppDbContext context, ILogger<UsuarioService> logger)
        {
            _context = context;
            _logger  = logger;
        }

        public async Task<PagedResultDto<ResponseUserDto>> ListarTodos(PaginationParams pagination)
        {
            var total = await _context.Usuarios.CountAsync();
            var items = await _context.Usuarios
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(u => ToResponse(u))
                .ToListAsync();

            return new PagedResultDto<ResponseUserDto>
            {
                Items      = items,
                Page       = pagination.Page,
                PageSize   = pagination.PageSize,
                TotalCount = total
            };
        }

        public async Task<ResponseUserDto?> BuscarPorId(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            return usuario is null ? null : ToResponse(usuario);
        }

        public async Task<ResponseUserDto> Criar(CreateUserDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {
                _logger.LogWarning("Tentativa de cadastro com email já existente: {Email}", dto.Email);
                throw new InvalidOperationException("Email já cadastrado.");
            }

            var usuario = new Usuario
            {
                Nome  = dto.Nome,
                Email = dto.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Role  = dto.Role
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Usuário criado: Id={Id}, Email={Email}", usuario.Id_usuario, usuario.Email);
            return ToResponse(usuario);
        }

        public async Task<ResponseUserDto> Atualizar(int id, UpdateUserDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario is null)
            {
                _logger.LogWarning("Usuário não encontrado para atualização: Id={Id}", id);
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id_usuario != id))
            {
                _logger.LogWarning("Email em conflito na atualização do usuário Id={Id}: {Email}", id, dto.Email);
                throw new InvalidOperationException("Email já está em uso por outro usuário.");
            }

            usuario.Nome  = dto.Nome;
            usuario.Email = dto.Email;
            usuario.Role  = dto.Role;

            if (!string.IsNullOrWhiteSpace(dto.Senha))
            {
                if (dto.Senha.Length < 8)
                    throw new InvalidOperationException("Senha deve ter no mínimo 8 caracteres.");
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Usuário atualizado: Id={Id}", id);
            return ToResponse(usuario);
        }

        public async Task Deletar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario is null)
            {
                _logger.LogWarning("Usuário não encontrado para exclusão: Id={Id}", id);
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Usuário excluído: Id={Id}", id);
        }

        private static ResponseUserDto ToResponse(Usuario u) => new()
        {
            Id_usuario = u.Id_usuario,
            Nome       = u.Nome,
            Email      = u.Email,
            Role       = u.Role
        };
    }
}

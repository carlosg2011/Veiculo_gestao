using Gestao_veiculos.Data;
using Gestao_veiculos.DTOs;
using Gestao_veiculos.Models;

namespace Gestao_veiculos.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ResponseUserDto> ListarTodos()
        {
            return _context.Usuarios
                .Select(u => ToResponse(u))
                .ToList();
        }

        public ResponseUserDto? BuscarPorId(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            return usuario is null ? null : ToResponse(usuario);
        }

        public ResponseUserDto Criar(CreateUserDto dto)
        {
            if (_context.Usuarios.Any(u => u.Email == dto.Email))
                throw new InvalidOperationException("Email já cadastrado.");

            var usuario = new Usuario
            {
                Nome  = dto.Nome,
                Email = dto.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return ToResponse(usuario);
        }

        public ResponseUserDto Atualizar(int id, CreateUserDto dto)
        {
            var usuario = _context.Usuarios.Find(id)
                ?? throw new KeyNotFoundException("Usuário não encontrado.");

            if (_context.Usuarios.Any(u => u.Email == dto.Email && u.Id_usuario != id))
                throw new InvalidOperationException("Email já está em uso por outro usuário.");

            usuario.Nome  = dto.Nome;
            usuario.Email = dto.Email;
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            _context.SaveChanges();

            return ToResponse(usuario);
        }

        public void Deletar(int id)
        {
            var usuario = _context.Usuarios.Find(id)
                ?? throw new KeyNotFoundException("Usuário não encontrado.");

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }

        private static ResponseUserDto ToResponse(Usuario u) => new()
        {
            Id_usuario = u.Id_usuario,
            Nome       = u.Nome,
            Email      = u.Email
        };
    }
}

using Microsoft.EntityFrameworkCore;
using Gestao_veiculos.Models;

namespace Gestao_veiculos.Data

{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> User => Set<User>();

    }
}

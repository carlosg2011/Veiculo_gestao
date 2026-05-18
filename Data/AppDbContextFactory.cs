using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gestao_veiculos.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(
                "server=localhost;port=3306;database=gestao_veiculos;user=root;password=;",
                new MySqlServerVersion(new Version(8, 0, 0)));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}

using Gestao_veiculos.Enums;
using Gestao_veiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestao_veiculos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Proprietario> Proprietarios { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Proposta> Propostas { get; set; }
        public DbSet<Vistoria> Vistorias { get; set; }
        public DbSet<Termo> Termos { get; set; }
        public DbSet<VistoriaFoto> VistoriaFotos { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.HasKey(u => u.Id_usuario);

                entity.Property(u => u.Id_usuario)
                    .HasColumnName("id_usuario");

                entity.Property(u => u.Nome)
                    .HasColumnName("nome")
                    .HasMaxLength(80)
                    .IsRequired();

                entity.Property(u => u.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.Property(u => u.Senha)
                    .HasColumnName("senha")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(u => u.Role)
                    .HasColumnName("role")
                    .HasMaxLength(20)
                    .IsRequired()
                    .HasDefaultValue("User");
            });

            modelBuilder.Entity<Proprietario>(entity =>
            {
                entity.ToTable("proprietario");

                entity.HasKey(p => p.Id_proprietario);

                entity.Property(p => p.Id_proprietario)
                    .HasColumnName("id_proprietario");

                entity.Property(p => p.Nome)
                    .HasColumnName("nome")
                    .HasMaxLength(80)
                    .IsRequired();

                entity.Property(p => p.Cpf)
                    .HasColumnName("cpf_cnpj")
                    .HasMaxLength(14)
                    .IsRequired();

                entity.HasIndex(p => p.Cpf)
                    .IsUnique();

                entity.Property(p => p.Telefone)
                    .HasColumnName("telefone")
                    .HasMaxLength(15)
                    .IsRequired();

                entity.Property(p => p.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            modelBuilder.Entity<Veiculo>(entity =>
            {
                entity.ToTable("veiculo");

                entity.HasKey(v => v.Id_veiculo);

                entity.Property(v => v.Id_veiculo)
                    .HasColumnName("id_veiculo");

                entity.Property(v => v.Placa)
                    .HasColumnName("placa")
                    .HasMaxLength(10)
                    .IsRequired();

                entity.HasIndex(v => v.Placa)
                    .IsUnique();

                entity.Property(v => v.Marca)
                    .HasColumnName("marca")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(v => v.Modelo)
                    .HasColumnName("modelo")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(v => v.AnoFab)
                    .HasColumnName("ano_fabricacao")
                    .IsRequired();

                entity.Property(v => v.AnoMod)
                    .HasColumnName("ano_modelo")
                    .IsRequired();

                entity.Property(v => v.Chassi)
                    .HasColumnName("chassi")
                    .HasMaxLength(17)
                    .IsRequired();

                entity.HasIndex(v => v.Chassi)
                    .IsUnique();

                entity.Property(v => v.Renavam)
                    .HasColumnName("renavam")
                    .HasMaxLength(11)
                    .IsRequired();

                entity.HasIndex(v => v.Renavam)
                    .IsUnique();

                entity.Property(v => v.Cor)
                    .HasColumnName("cor")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(v => v.Status)
                    .HasColumnName("status_veiculo")
                    .HasMaxLength(20)
                    .IsRequired()
                    .HasConversion<string>();
            });

            modelBuilder.Entity<Proposta>(entity =>
            {
                entity.ToTable("proposta");

                entity.HasKey(p => p.Id_proposta);

                entity.Property(p => p.Id_proposta)
                    .HasColumnName("id_proposta");

                entity.Property(p => p.SessaoProposta)
                    .HasColumnName("cod_proposta")
                    .HasMaxLength(25)
                    .IsRequired();

                entity.Property(p => p.DataCriacao)
                    .HasColumnName("data_criacao")
                    .IsRequired();

                entity.Property(p => p.Status)
                    .HasColumnName("status_proposta")
                    .HasMaxLength(30)
                    .IsRequired()
                    .HasConversion<string>();

                entity.Property(p => p.Id_usuario)
                    .HasColumnName("id_usuario")
                    .IsRequired();

                entity.Property(p => p.Id_veiculo)
                    .HasColumnName("id_veiculo")
                    .IsRequired();

                entity.Property(p => p.Id_proprietario)
                    .HasColumnName("id_proprietario")
                    .IsRequired();

                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(p => p.Id_usuario)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Veiculo>()
                    .WithMany()
                    .HasForeignKey(p => p.Id_veiculo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Proprietario>()
                    .WithMany()
                    .HasForeignKey(p => p.Id_proprietario)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Vistoria>(entity =>
            {
                entity.ToTable("vistoria");

                entity.HasKey(v => v.Id_vistoria);

                entity.Property(v => v.Id_vistoria)
                    .HasColumnName("id_vistoria")
                    .ValueGeneratedNever();

                entity.Property(v => v.DataSolicitacao)
                    .HasColumnName("data_solicitacao")
                    .IsRequired();

                entity.Property(v => v.DataInicio)
                    .HasColumnName("data_inicio");

                entity.Property(v => v.DataConclusao)
                    .HasColumnName("data_conclusao");

                entity.Property(v => v.Status)
                    .HasColumnName("status_vistoria")
                    .HasMaxLength(30)
                    .IsRequired()
                    .HasConversion<string>();

                entity.Property(v => v.Id_proposta)
                    .HasColumnName("id_proposta")
                    .IsRequired();

                entity.Property(v => v.Id_usuario)
                    .HasColumnName("id_usuario_responsavel")
                    .IsRequired();

                entity.Property(v => v.Observacoes)
                    .HasColumnName("observacoes")
                    .HasMaxLength(1000);

                entity.HasOne<Proposta>()
                    .WithMany()
                    .HasForeignKey(v => v.Id_proposta)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(v => v.Id_usuario)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<VistoriaFoto>(entity =>
            {
                entity.ToTable("vistoria_foto");

                entity.HasKey(f => f.Id_foto);

                entity.Property(f => f.Id_foto)
                    .HasColumnName("id_foto");

                entity.Property(f => f.Id_vistoria)
                    .HasColumnName("id_vistoria")
                    .IsRequired();

                entity.Property(f => f.Slot)
                    .HasColumnName("slot")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(f => f.Url)
                    .HasColumnName("url")
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(f => f.Verdict)
                    .HasColumnName("verdict")
                    .HasMaxLength(20);

                entity.HasIndex(f => new { f.Id_vistoria, f.Slot })
                    .IsUnique();

                entity.HasOne<Vistoria>()
                    .WithMany()
                    .HasForeignKey(f => f.Id_vistoria)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Termo>(entity =>
            {
                entity.ToTable("termo");

                entity.HasKey(t => t.Id_termo);

                entity.Property(t => t.Id_termo)
                    .HasColumnName("id_termo");

                entity.Property(t => t.NumeroTermo)
                    .HasColumnName("numero_termo")
                    .HasMaxLength(25)
                    .IsRequired();

                entity.Property(t => t.Status)
                    .HasColumnName("status_termo")
                    .HasMaxLength(30)
                    .IsRequired()
                    .HasConversion<string>();

                entity.Property(t => t.DataEnvio)
                    .HasColumnName("data_envio")
                    .IsRequired();

                entity.Property(t => t.DataAssinatura)
                    .HasColumnName("data_assinatura");

                entity.Property(t => t.Id_proposta)
                    .HasColumnName("id_proposta")
                    .IsRequired();

                entity.HasOne<Proposta>()
                    .WithMany()
                    .HasForeignKey(t => t.Id_proposta)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PasswordResetToken>(entity =>
            {
                entity.ToTable("password_reset_token");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).HasColumnName("id");
                entity.Property(t => t.Id_usuario).HasColumnName("id_usuario").IsRequired();
                entity.Property(t => t.Token).HasColumnName("token").HasMaxLength(128).IsRequired();
                entity.HasIndex(t => t.Token).IsUnique();
                entity.Property(t => t.ExpiresAt).HasColumnName("expires_at").IsRequired();
                entity.Property(t => t.Used).HasColumnName("used").HasDefaultValue(false);
                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(t => t.Id_usuario)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

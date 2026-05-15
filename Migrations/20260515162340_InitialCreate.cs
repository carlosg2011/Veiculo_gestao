using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veiculo_gestao.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "proprietario",
                columns: table => new
                {
                    id_proprietario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cpf_cnpj = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proprietario", x => x.id_proprietario);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    senha = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id_usuario);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "veiculo",
                columns: table => new
                {
                    id_veiculo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    placa = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    marca = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    modelo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ano_fabricacao = table.Column<int>(type: "int", nullable: false),
                    ano_modelo = table.Column<int>(type: "int", nullable: false),
                    chassi = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    renavam = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cor = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status_veiculo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_veiculo", x => x.id_veiculo);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "proposta",
                columns: table => new
                {
                    id_proposta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cod_proposta = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_criacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status_proposta = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_veiculo = table.Column<int>(type: "int", nullable: false),
                    id_proprietario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proposta", x => x.id_proposta);
                    table.ForeignKey(
                        name: "FK_proposta_proprietario_id_proprietario",
                        column: x => x.id_proprietario,
                        principalTable: "proprietario",
                        principalColumn: "id_proprietario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_proposta_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_proposta_veiculo_id_veiculo",
                        column: x => x.id_veiculo,
                        principalTable: "veiculo",
                        principalColumn: "id_veiculo",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "termo",
                columns: table => new
                {
                    id_termo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    numero_termo = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status_termo = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_envio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    data_assinatura = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    id_proposta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_termo", x => x.id_termo);
                    table.ForeignKey(
                        name: "FK_termo_proposta_id_proposta",
                        column: x => x.id_proposta,
                        principalTable: "proposta",
                        principalColumn: "id_proposta",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vistoria",
                columns: table => new
                {
                    id_vistoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    data_solicitacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    data_inicio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    data_conclusao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    status_vistoria = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_proposta = table.Column<int>(type: "int", nullable: false),
                    id_usuario_responsavel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vistoria", x => x.id_vistoria);
                    table.ForeignKey(
                        name: "FK_vistoria_proposta_id_proposta",
                        column: x => x.id_proposta,
                        principalTable: "proposta",
                        principalColumn: "id_proposta",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vistoria_usuario_id_usuario_responsavel",
                        column: x => x.id_usuario_responsavel,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_proposta_id_proprietario",
                table: "proposta",
                column: "id_proprietario");

            migrationBuilder.CreateIndex(
                name: "IX_proposta_id_usuario",
                table: "proposta",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_proposta_id_veiculo",
                table: "proposta",
                column: "id_veiculo");

            migrationBuilder.CreateIndex(
                name: "IX_proprietario_cpf_cnpj",
                table: "proprietario",
                column: "cpf_cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_termo_id_proposta",
                table: "termo",
                column: "id_proposta");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_email",
                table: "usuario",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_veiculo_chassi",
                table: "veiculo",
                column: "chassi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_veiculo_placa",
                table: "veiculo",
                column: "placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_veiculo_renavam",
                table: "veiculo",
                column: "renavam",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vistoria_id_proposta",
                table: "vistoria",
                column: "id_proposta");

            migrationBuilder.CreateIndex(
                name: "IX_vistoria_id_usuario_responsavel",
                table: "vistoria",
                column: "id_usuario_responsavel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "termo");

            migrationBuilder.DropTable(
                name: "vistoria");

            migrationBuilder.DropTable(
                name: "proposta");

            migrationBuilder.DropTable(
                name: "proprietario");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "veiculo");
        }
    }
}

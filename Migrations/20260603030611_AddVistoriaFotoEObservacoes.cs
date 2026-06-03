using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veiculo_gestao.Migrations
{
    /// <inheritdoc />
    public partial class AddVistoriaFotoEObservacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "observacoes",
                table: "vistoria",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vistoria_foto",
                columns: table => new
                {
                    id_foto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_vistoria = table.Column<int>(type: "int", nullable: false),
                    slot = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    verdict = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vistoria_foto", x => x.id_foto);
                    table.ForeignKey(
                        name: "FK_vistoria_foto_vistoria_id_vistoria",
                        column: x => x.id_vistoria,
                        principalTable: "vistoria",
                        principalColumn: "id_vistoria",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_vistoria_foto_id_vistoria_slot",
                table: "vistoria_foto",
                columns: new[] { "id_vistoria", "slot" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vistoria_foto");

            migrationBuilder.DropColumn(
                name: "observacoes",
                table: "vistoria");
        }
    }
}

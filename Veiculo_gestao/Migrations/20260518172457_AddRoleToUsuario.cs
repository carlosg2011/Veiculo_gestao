using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veiculo_gestao.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "usuario",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "User")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "usuario");
        }
    }
}

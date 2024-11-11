using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OT.Assessment.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddedGameNameColoumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GameName",
                table: "CasinoWagers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameName",
                table: "CasinoWagers");
        }
    }
}

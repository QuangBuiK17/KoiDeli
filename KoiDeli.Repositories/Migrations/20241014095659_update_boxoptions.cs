using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiDeli.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class update_boxoptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Quantity",
                table: "BoxOptions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "BoxOptions");
        }
    }
}

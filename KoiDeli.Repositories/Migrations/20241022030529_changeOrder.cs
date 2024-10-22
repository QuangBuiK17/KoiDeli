using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiDeli.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class changeOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalShippingFee",
                table: "Orders",
                newName: "TotalFee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalFee",
                table: "Orders",
                newName: "TotalShippingFee");
        }
    }
}

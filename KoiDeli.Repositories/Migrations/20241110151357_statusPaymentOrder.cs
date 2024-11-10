using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiDeli.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class statusPaymentOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPayment",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPayment",
                table: "Orders");
        }
    }
}

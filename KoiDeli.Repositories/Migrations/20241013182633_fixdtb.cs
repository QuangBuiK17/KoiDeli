using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiDeli.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class fixdtb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_DistanceId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ParnerShipmentId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "UrlAvatar",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DistanceId",
                table: "Orders",
                column: "DistanceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ParnerShipmentId",
                table: "OrderDetails",
                column: "ParnerShipmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_DistanceId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ParnerShipmentId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "UrlAvatar",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DistanceId",
                table: "Orders",
                column: "DistanceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ParnerShipmentId",
                table: "OrderDetails",
                column: "ParnerShipmentId",
                unique: true);
        }
    }
}

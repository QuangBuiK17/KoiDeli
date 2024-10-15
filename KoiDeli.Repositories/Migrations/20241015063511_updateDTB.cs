using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiDeli.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class updateDTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_PartnerShipment_ParnerShipmentId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Distances_DistanceId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "PartnerShipment");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DistanceId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_BoxOptionId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "DistanceId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingEnd",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingStart",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalPackingFee",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalShipment",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LocalShipingFee",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "PartnerShippingFee",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "ParnerShipmentId",
                table: "OrderDetails",
                newName: "DistanceId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ParnerShipmentId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_DistanceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_BoxOptionId",
                table: "OrderDetails",
                column: "BoxOptionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Distances_DistanceId",
                table: "OrderDetails",
                column: "DistanceId",
                principalTable: "Distances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Distances_DistanceId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_BoxOptionId",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "DistanceId",
                table: "OrderDetails",
                newName: "ParnerShipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_DistanceId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ParnerShipmentId");

            migrationBuilder.AddColumn<int>(
                name: "DistanceId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingEnd",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingStart",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingTime",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TotalPackingFee",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalShipment",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LocalShipingFee",
                table: "OrderDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PartnerShippingFee",
                table: "OrderDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "PartnerShipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Completed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificationBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    StartDay = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerShipment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DistanceId",
                table: "Orders",
                column: "DistanceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_BoxOptionId",
                table: "OrderDetails",
                column: "BoxOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_PartnerShipment_ParnerShipmentId",
                table: "OrderDetails",
                column: "ParnerShipmentId",
                principalTable: "PartnerShipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Distances_DistanceId",
                table: "Orders",
                column: "DistanceId",
                principalTable: "Distances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

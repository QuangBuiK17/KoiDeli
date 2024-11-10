using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiDeli.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class vnpayUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WalletType",
                table: "Wallets");

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDate",
                table: "Transaction",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionStatus",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TransactionStatus",
                table: "Transaction");

            migrationBuilder.AddColumn<string>(
                name: "WalletType",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

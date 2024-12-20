using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DairyManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class BuyerReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Seller",
                table: "Sales",
                newName: "SellerId");

            migrationBuilder.RenameColumn(
                name: "Buyer",
                table: "Sales",
                newName: "BuyerId");

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyer",
                table: "SupplierCustomers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeller",
                table: "SupplierCustomers",
                type: "bit",
                nullable: false,
                defaultValue: false);
          }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBuyer",
                table: "SupplierCustomers");

            migrationBuilder.DropColumn(
                name: "IsSeller",
                table: "SupplierCustomers");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Sales",
                newName: "Seller");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Sales",
                newName: "Buyer");

        }
    }
}

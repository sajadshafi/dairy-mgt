using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DairyManagementSystem.Migrations {
   /// <inheritdoc />
   public partial class AddedProductCurrentPriceAndTotalAmount : Migration {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder) {

         migrationBuilder.AddColumn<decimal>(
             name: "CurrentPrice",
             table: "Sales",
             type: "decimal(18,2)",
             nullable: false,
             defaultValue: 0m);

         migrationBuilder.AddColumn<decimal>(
             name: "TotalAmount",
             table: "Sales",
             type: "decimal(18,2)",
             nullable: false,
             defaultValue: 0m);
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder) {

         migrationBuilder.DropColumn(
             name: "CurrentPrice",
             table: "Sales");

         migrationBuilder.DropColumn(
             name: "TotalAmount",
             table: "Sales");

      }
   }
}

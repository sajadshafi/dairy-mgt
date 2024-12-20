using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DairyManagementSystem.Migrations {
   /// <inheritdoc />
   public partial class AddedDateToSaleEntity : Migration {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder) {
         migrationBuilder.AddColumn<DateTime>(
             name: "Date",
             table: "Sales",
             type: "datetime2",
             nullable: false,
             defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

         migrationBuilder.CreateIndex(
             name: "IX_Sales_BuyerId",
             table: "Sales",
             column: "BuyerId");

         migrationBuilder.AddForeignKey(
             name: "FK_Sales_AspNetUsers_BuyerId",
             table: "Sales",
             column: "BuyerId",
             principalTable: "AspNetUsers",
             principalColumn: "Id",
             onDelete: ReferentialAction.Cascade);
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder) {
         migrationBuilder.DropForeignKey(
             name: "FK_Sales_AspNetUsers_BuyerId",
             table: "Sales");

         migrationBuilder.DropIndex(
             name: "IX_Sales_BuyerId",
             table: "Sales");
      }
   }
}

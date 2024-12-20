using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DairyManagementSystem.Migrations {
   /// <inheritdoc />
   public partial class AddedIdToSupplierCustomerEntity : Migration {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder) {
         migrationBuilder.AddColumn<Guid>(
             name: "Id",
             table: "SupplierCustomers",
             type: "uniqueidentifier",
             nullable: false,
             defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

         migrationBuilder.AddPrimaryKey(
             name: "PK_SupplierCustomers",
             table: "SupplierCustomers",
             column: "Id");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder) {
         migrationBuilder.DropPrimaryKey(
             name: "PK_SupplierCustomers",
             table: "SupplierCustomers");

         migrationBuilder.DropColumn(
             name: "Id",
             table: "SupplierCustomers");
      }
   }
}

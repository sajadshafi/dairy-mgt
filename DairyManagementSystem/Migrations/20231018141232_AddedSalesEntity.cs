using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DairyManagementSystem.Migrations {
   /// <inheritdoc />
   public partial class AddedSalesEntity : Migration {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder) {

         migrationBuilder.CreateTable(
             name: "Sales",
             columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Seller = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Buyer = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false),
                ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false)
             },
             constraints: table => {
                table.PrimaryKey("PK_Sales", x => x.Id);
                table.ForeignKey(
                       name: "FK_Sales_Products_ProductId",
                       column: x => x.ProductId,
                       principalTable: "Products",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
             });


         migrationBuilder.CreateIndex(
             name: "IX_Sales_ProductId",
             table: "Sales",
             column: "ProductId");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder) {
         migrationBuilder.DropTable(
             name: "Sales");

      }
   }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DairyManagementSystem.Migrations {
   /// <inheritdoc />
   public partial class AddedProfileForUser : Migration {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder) {
         migrationBuilder.AddColumn<byte[]>(
             name: "ProfileImage",
             table: "AspNetUsers",
             type: "varbinary(max)",
             nullable: true);
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder) {
         migrationBuilder.DropColumn(
             name: "ProfileImage",
             table: "AspNetUsers");
      }
   }
}

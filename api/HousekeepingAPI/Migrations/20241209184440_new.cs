using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HousekeepingAPI.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_AspNetUsers_UserId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_UserId",
                table: "Services");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a2fa505-4018-4ce8-a53f-1ce164a77b7d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5db66f03-004b-42d8-ac79-81b425a2945a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6aed2340-7303-41e3-a716-19e6cc6e6542");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Services");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "185cbe1a-ffd5-4a96-bf88-0fff1c5aeb4e", null, "User", "USER" },
                    { "2294ec4f-c34b-4ebb-a4a7-b3c4dc629353", null, "Admin", "ADMIN" },
                    { "f7f5597c-da45-4bf6-964e-2e05ad15f69d", null, "Provider", "PROVIDER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "185cbe1a-ffd5-4a96-bf88-0fff1c5aeb4e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2294ec4f-c34b-4ebb-a4a7-b3c4dc629353");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7f5597c-da45-4bf6-964e-2e05ad15f69d");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Services",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5a2fa505-4018-4ce8-a53f-1ce164a77b7d", null, "User", "USER" },
                    { "5db66f03-004b-42d8-ac79-81b425a2945a", null, "Admin", "ADMIN" },
                    { "6aed2340-7303-41e3-a716-19e6cc6e6542", null, "Provider", "PROVIDER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_UserId",
                table: "Services",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_AspNetUsers_UserId",
                table: "Services",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

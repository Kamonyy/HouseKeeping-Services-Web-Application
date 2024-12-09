using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HousekeepingAPI.Migrations
{
    /// <inheritdoc />
    public partial class ServicedUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b14dfa03-2767-41eb-b736-5d4a2bd0e96a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b65918c5-c4f2-413f-9647-ee7fd8717b46");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bde53ba0-772e-4b58-8870-84e46c4e3c3d");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "69289cab-16a8-4031-b720-dfcf5e06241e", null, "Provider", "PROVIDER" },
                    { "8daf3f3d-d0a1-4f03-9399-4b7956953d05", null, "User", "USER" },
                    { "9832279a-b76f-42a9-ae4a-dca70ddbbb99", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69289cab-16a8-4031-b720-dfcf5e06241e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8daf3f3d-d0a1-4f03-9399-4b7956953d05");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9832279a-b76f-42a9-ae4a-dca70ddbbb99");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Services");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b14dfa03-2767-41eb-b736-5d4a2bd0e96a", null, "User", "USER" },
                    { "b65918c5-c4f2-413f-9647-ee7fd8717b46", null, "Provider", "PROVIDER" },
                    { "bde53ba0-772e-4b58-8870-84e46c4e3c3d", null, "Admin", "ADMIN" }
                });
        }
    }
}

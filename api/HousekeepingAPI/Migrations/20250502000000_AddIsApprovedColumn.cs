using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HousekeepingAPI.Migrations
{
    /// <summary>
    /// Migration to add IsApproved column to Services table
    /// </summary>
    public partial class AddIsApprovedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Services",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Services");
        }
    }
} 
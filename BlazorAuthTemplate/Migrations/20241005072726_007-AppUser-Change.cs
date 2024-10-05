using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorAuthTemplate.Migrations
{
    /// <inheritdoc />
    public partial class _007AppUserChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}

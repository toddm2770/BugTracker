using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorAuthTemplate.Migrations
{
    /// <inheritdoc />
    public partial class _003TicketAttachmentFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Images_ImageId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachments_Images_FileUploadId",
                table: "TicketAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "Uploads");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Uploads",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Uploads",
                table: "Uploads",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Uploads_ImageId",
                table: "AspNetUsers",
                column: "ImageId",
                principalTable: "Uploads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Uploads_ImageId",
                table: "Companies",
                column: "ImageId",
                principalTable: "Uploads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAttachments_Uploads_FileUploadId",
                table: "TicketAttachments",
                column: "FileUploadId",
                principalTable: "Uploads",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Uploads_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Uploads_ImageId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachments_Uploads_FileUploadId",
                table: "TicketAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Uploads",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Uploads");

            migrationBuilder.RenameTable(
                name: "Uploads",
                newName: "Images");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Images_ImageId",
                table: "Companies",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAttachments_Images_FileUploadId",
                table: "TicketAttachments",
                column: "FileUploadId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}

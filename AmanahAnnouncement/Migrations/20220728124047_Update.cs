using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmanahAnnouncement.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileModel_Announcments_AnnouncmentId",
                table: "FileModel");

            migrationBuilder.DropIndex(
                name: "IX_FileModel_AnnouncmentId",
                table: "FileModel");

            migrationBuilder.DropColumn(
                name: "AnnouncmentId",
                table: "FileModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachement",
                table: "Announcments");

            migrationBuilder.AddColumn<int>(
                name: "AnnouncmentId",
                table: "FileModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileModel_AnnouncmentId",
                table: "FileModel",
                column: "AnnouncmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileModel_Announcments_AnnouncmentId",
                table: "FileModel",
                column: "AnnouncmentId",
                principalTable: "Announcments",
                principalColumn: "Id");
        }
    }
}

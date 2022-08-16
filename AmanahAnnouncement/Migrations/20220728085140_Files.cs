using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmanahAnnouncement.Migrations
{
    public partial class Files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachement",
                table: "Announcments");

            migrationBuilder.CreateTable(
                name: "FileModel",
                columns: table => new
                {
                    FileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnnId = table.Column<int>(type: "int", nullable: true),
                    AnnouncmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileModel", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_FileModel_Announcments_AnnouncmentId",
                        column: x => x.AnnouncmentId,
                        principalTable: "Announcments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileModel_AnnouncmentId",
                table: "FileModel",
                column: "AnnouncmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileModel");

            migrationBuilder.AddColumn<string>(
                name: "Attachement",
                table: "Announcments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

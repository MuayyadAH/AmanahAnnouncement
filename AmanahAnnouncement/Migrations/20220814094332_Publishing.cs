using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmanahAnnouncement.Migrations
{
    public partial class Publishing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnnouncementDate",
                table: "Announcments",
                newName: "CreationDate");

            migrationBuilder.AddColumn<bool>(
                name: "isArchived",
                table: "News",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isPublished",
                table: "News",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isArchived",
                table: "Announcments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isPublished",
                table: "Announcments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isArchived",
                table: "News");

            migrationBuilder.DropColumn(
                name: "isPublished",
                table: "News");

            migrationBuilder.DropColumn(
                name: "isArchived",
                table: "Announcments");

            migrationBuilder.DropColumn(
                name: "isPublished",
                table: "Announcments");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Announcments",
                newName: "AnnouncementDate");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedprDB.Migrations
{
    public partial class NotificationIdAppointments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotificationId",
                table: "Drugs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "Drugs");
        }
    }
}

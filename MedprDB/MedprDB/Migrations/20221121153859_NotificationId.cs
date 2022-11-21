using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedprDB.Migrations
{
    public partial class NotificationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "Drugs");

            migrationBuilder.AddColumn<string>(
                name: "NotificationId",
                table: "Vaccinations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationId",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "Vaccinations");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "NotificationId",
                table: "Drugs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

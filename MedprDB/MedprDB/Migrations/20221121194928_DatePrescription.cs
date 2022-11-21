using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedprDB.Migrations
{
    public partial class DatePrescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Prescriptions",
                newName: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Prescriptions",
                newName: "StartDate");
        }
    }
}

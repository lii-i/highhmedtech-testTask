using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace highhmedtech_test.Migrations
{
    /// <inheritdoc />
    public partial class AddPassportAndSNILS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SNILS",
                table: "Patients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Passport",
                table: "Doctors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SNILS",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Passport",
                table: "Doctors");
        }
    }
}

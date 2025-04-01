using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatCatalog.Migrations
{
    /// <inheritdoc />
    public partial class Completed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Job_Status",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Job");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Job",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Job_IsCompleted",
                table: "Job",
                column: "IsCompleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Job_IsCompleted",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Job");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Job",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Job_Status",
                table: "Job",
                column: "Status");
        }
    }
}

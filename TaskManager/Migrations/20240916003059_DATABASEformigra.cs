using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class DATABASEformigra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_AssignedId",
                table: "Tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_AssignedId",
                table: "Tasks",
                column: "AssignedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_AssignedId",
                table: "Tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_AssignedId",
                table: "Tasks",
                column: "AssignedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

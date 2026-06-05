using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patients.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTherapistProfileUserForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_TherapistProfiles_Users_UserId",
                table: "TherapistProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TherapistProfiles_Users_UserId",
                table: "TherapistProfiles");
        }
    }
}

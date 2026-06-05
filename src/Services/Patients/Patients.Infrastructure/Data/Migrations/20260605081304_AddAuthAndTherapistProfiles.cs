using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patients.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthAndTherapistProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TherapistProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfessionalName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Profession = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Specialties = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProfessionalNumber = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Workplace = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: true),
                    ReportSignature = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TherapistProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TherapistProfiles_UserId",
                table: "TherapistProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TherapistProfiles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

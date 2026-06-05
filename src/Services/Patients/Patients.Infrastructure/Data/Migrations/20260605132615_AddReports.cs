using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patients.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: false),
                    TherapistId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PatientSnapshot = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ExecutiveSummary = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    AttendanceSummary = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    GoalProgressSummary = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    SessionSummary = table.Column<string>(type: "character varying(6000)", maxLength: 6000, nullable: false),
                    Recommendations = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    AdditionalNotes = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    PdfContent = table.Column<byte[]>(type: "bytea", nullable: true),
                    PdfFileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PdfExportedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WordContent = table.Column<byte[]>(type: "bytea", nullable: true),
                    WordFileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    WordExportedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TherapistId_PatientId_GeneratedAt",
                table: "Reports",
                columns: new[] { "TherapistId", "PatientId", "GeneratedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}

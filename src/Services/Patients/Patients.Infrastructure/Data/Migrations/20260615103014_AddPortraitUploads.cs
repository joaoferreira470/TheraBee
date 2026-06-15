using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patients.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPortraitUploads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PortraitContentType",
                table: "TherapistProfiles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortraitStorageKey",
                table: "TherapistProfiles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PortraitUpdatedAt",
                table: "TherapistProfiles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortraitContentType",
                table: "Patients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortraitStorageKey",
                table: "Patients",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PortraitUpdatedAt",
                table: "Patients",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PortraitContentType",
                table: "TherapistProfiles");

            migrationBuilder.DropColumn(
                name: "PortraitStorageKey",
                table: "TherapistProfiles");

            migrationBuilder.DropColumn(
                name: "PortraitUpdatedAt",
                table: "TherapistProfiles");

            migrationBuilder.DropColumn(
                name: "PortraitContentType",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PortraitStorageKey",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PortraitUpdatedAt",
                table: "Patients");
        }
    }
}

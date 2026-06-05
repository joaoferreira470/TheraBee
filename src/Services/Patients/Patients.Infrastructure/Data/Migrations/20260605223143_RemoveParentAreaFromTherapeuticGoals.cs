using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patients.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveParentAreaFromTherapeuticGoals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TherapeuticGoals_TherapeuticGoals_ParentAreaId",
                table: "TherapeuticGoals");

            migrationBuilder.DropIndex(
                name: "IX_TherapeuticGoals_ParentAreaId",
                table: "TherapeuticGoals");

            migrationBuilder.DropColumn(
                name: "ParentAreaId",
                table: "TherapeuticGoals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentAreaId",
                table: "TherapeuticGoals",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TherapeuticGoals_ParentAreaId",
                table: "TherapeuticGoals",
                column: "ParentAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_TherapeuticGoals_TherapeuticGoals_ParentAreaId",
                table: "TherapeuticGoals",
                column: "ParentAreaId",
                principalTable: "TherapeuticGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

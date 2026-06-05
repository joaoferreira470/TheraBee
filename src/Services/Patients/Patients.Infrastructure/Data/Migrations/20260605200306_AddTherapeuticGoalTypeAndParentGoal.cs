using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patients.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTherapeuticGoalTypeAndParentGoal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentGoalId",
                table: "TherapeuticGoals",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "TherapeuticGoals",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "ShortTerm");

            migrationBuilder.CreateIndex(
                name: "IX_TherapeuticGoals_ParentGoalId",
                table: "TherapeuticGoals",
                column: "ParentGoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_TherapeuticGoals_TherapeuticGoals_ParentGoalId",
                table: "TherapeuticGoals",
                column: "ParentGoalId",
                principalTable: "TherapeuticGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TherapeuticGoals_TherapeuticGoals_ParentGoalId",
                table: "TherapeuticGoals");

            migrationBuilder.DropIndex(
                name: "IX_TherapeuticGoals_ParentGoalId",
                table: "TherapeuticGoals");

            migrationBuilder.DropColumn(
                name: "ParentGoalId",
                table: "TherapeuticGoals");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "TherapeuticGoals");
        }
    }
}

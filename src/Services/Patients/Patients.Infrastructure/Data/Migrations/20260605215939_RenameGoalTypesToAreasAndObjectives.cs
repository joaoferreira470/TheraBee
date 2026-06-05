using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patients.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameGoalTypesToAreasAndObjectives : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TherapeuticGoals_TherapeuticGoals_ParentGoalId",
                table: "TherapeuticGoals");

            migrationBuilder.RenameColumn(
                name: "ParentGoalId",
                table: "TherapeuticGoals",
                newName: "ParentAreaId");

            migrationBuilder.RenameIndex(
                name: "IX_TherapeuticGoals_ParentGoalId",
                table: "TherapeuticGoals",
                newName: "IX_TherapeuticGoals_ParentAreaId");

            migrationBuilder.Sql("""
                UPDATE "TherapeuticGoals"
                SET "Type" = CASE "Type"
                    WHEN 'LongTerm' THEN 'Area'
                    WHEN 'ShortTerm' THEN 'Objective'
                    ELSE "Type"
                END;
                """);

            migrationBuilder.AddForeignKey(
                name: "FK_TherapeuticGoals_TherapeuticGoals_ParentAreaId",
                table: "TherapeuticGoals",
                column: "ParentAreaId",
                principalTable: "TherapeuticGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TherapeuticGoals_TherapeuticGoals_ParentAreaId",
                table: "TherapeuticGoals");

            migrationBuilder.RenameColumn(
                name: "ParentAreaId",
                table: "TherapeuticGoals",
                newName: "ParentGoalId");

            migrationBuilder.RenameIndex(
                name: "IX_TherapeuticGoals_ParentAreaId",
                table: "TherapeuticGoals",
                newName: "IX_TherapeuticGoals_ParentGoalId");

            migrationBuilder.Sql("""
                UPDATE "TherapeuticGoals"
                SET "Type" = CASE "Type"
                    WHEN 'Area' THEN 'LongTerm'
                    WHEN 'Objective' THEN 'ShortTerm'
                    ELSE "Type"
                END;
                """);

            migrationBuilder.AddForeignKey(
                name: "FK_TherapeuticGoals_TherapeuticGoals_ParentGoalId",
                table: "TherapeuticGoals",
                column: "ParentGoalId",
                principalTable: "TherapeuticGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

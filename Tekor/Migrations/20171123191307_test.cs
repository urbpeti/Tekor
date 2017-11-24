using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Tekor.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActualGoalState_Goal_goalID",
                table: "ActualGoalState");

            migrationBuilder.RenameColumn(
                name: "goalID",
                table: "ActualGoalState",
                newName: "GoalID");

            migrationBuilder.RenameIndex(
                name: "IX_ActualGoalState_goalID",
                table: "ActualGoalState",
                newName: "IX_ActualGoalState_GoalID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActualGoalState_Goal_GoalID",
                table: "ActualGoalState",
                column: "GoalID",
                principalTable: "Goal",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActualGoalState_Goal_GoalID",
                table: "ActualGoalState");

            migrationBuilder.RenameColumn(
                name: "GoalID",
                table: "ActualGoalState",
                newName: "goalID");

            migrationBuilder.RenameIndex(
                name: "IX_ActualGoalState_GoalID",
                table: "ActualGoalState",
                newName: "IX_ActualGoalState_goalID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActualGoalState_Goal_goalID",
                table: "ActualGoalState",
                column: "goalID",
                principalTable: "Goal",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

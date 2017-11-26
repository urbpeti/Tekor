using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Tekor.Migrations
{
    public partial class refactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Reward");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Goal",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "ActualGoalState",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Goal");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "ActualGoalState");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Reward",
                nullable: true);
        }
    }
}

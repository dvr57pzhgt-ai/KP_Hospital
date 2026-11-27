using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KP_Hospital.Migrations
{
    /// <inheritdoc />
    public partial class AddSickLeaveFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SickLeaveEndDate",
                table: "MedicalRecord",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SickLeaveExtensionDays",
                table: "MedicalRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SickLeaveEndDate",
                table: "MedicalRecord");

            migrationBuilder.DropColumn(
                name: "SickLeaveExtensionDays",
                table: "MedicalRecord");
        }
    }
}

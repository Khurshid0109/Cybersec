using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cybersec.Data.Migrations
{
    /// <inheritdoc />
    public partial class DatetimeModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "Information",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Information",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Information");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Information",
                newName: "CreatedTime");
        }
    }
}

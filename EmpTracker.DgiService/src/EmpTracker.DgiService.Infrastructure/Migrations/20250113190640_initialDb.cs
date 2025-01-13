using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpTracker.DgiService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Designations",
                columns: table => new
                {
                    DesignationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DesignationName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DesignationKey = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designations", x => x.DesignationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Designations_DesignationKey",
                table: "Designations",
                column: "DesignationKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Designations");
        }
    }
}

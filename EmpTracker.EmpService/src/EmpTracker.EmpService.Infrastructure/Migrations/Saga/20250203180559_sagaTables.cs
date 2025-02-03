using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpTracker.EmpService.Infrastructure.Migrations.Saga
{
    /// <inheritdoc />
    public partial class sagaTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "saga");

            migrationBuilder.CreateTable(
                name: "EmployeeCreationStates",
                schema: "saga",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DesignationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentTotalEmployeeCountIncreased = table.Column<bool>(type: "bit", nullable: false),
                    DesignationTotalEmployeeCountIncreased = table.Column<bool>(type: "bit", nullable: false),
                    IdentityRetryCount = table.Column<int>(type: "int", nullable: false),
                    EmployeeRetryCount = table.Column<int>(type: "int", nullable: false),
                    DepartmentRetryCount = table.Column<int>(type: "int", nullable: false),
                    DesignationRetryCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCreationStates", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDeletionStates",
                schema: "saga",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DesignationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentTotalEmployeeCountDecreased = table.Column<bool>(type: "bit", nullable: false),
                    DesignationTotalEmployeeCountDecreased = table.Column<bool>(type: "bit", nullable: false),
                    IdentityRetryCount = table.Column<int>(type: "int", nullable: false),
                    DepartmentRetryCount = table.Column<int>(type: "int", nullable: false),
                    DesignationRetryCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDeletionStates", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeCreationStates",
                schema: "saga");

            migrationBuilder.DropTable(
                name: "EmployeeDeletionStates",
                schema: "saga");
        }
    }
}

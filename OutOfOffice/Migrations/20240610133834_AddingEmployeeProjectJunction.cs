using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutOfOffice.Migrations
{
    /// <inheritdoc />
    public partial class AddingEmployeeProjectJunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeProjectJunctions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProjectJunctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeProjectJunctions_AllEmployees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AllEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeProjectJunctions_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProjectJunctions_EmployeeId",
                table: "EmployeeProjectJunctions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProjectJunctions_ProjectId",
                table: "EmployeeProjectJunctions",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeProjectJunctions");
        }
    }
}

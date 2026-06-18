using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kipu.API.Shared.Infrastructure.Persistence.EFC.Migrations
{
    /// <inheritdoc />
    public partial class MakeBudgetLineIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE material_requests SET budget_line_id = NULL WHERE budget_line_id = 0");

            migrationBuilder.AlterColumn<int>(
                name: "budget_line_id",
                table: "material_requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "budget_line_id",
                table: "material_requests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

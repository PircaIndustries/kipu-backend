using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kipu.API.Shared.Infrastructure.Persistence.EFC.Migrations
{
    /// <inheritdoc />
    public partial class FixExistingBudgetLineIdZero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE material_requests SET budget_line_id = NULL WHERE budget_line_id = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE material_requests SET budget_line_id = 0 WHERE budget_line_id IS NULL");
        }
    }
}

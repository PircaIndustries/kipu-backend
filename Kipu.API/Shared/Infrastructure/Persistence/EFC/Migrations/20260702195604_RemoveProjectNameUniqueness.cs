using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Kipu.API.Shared.Infrastructure.Persistence.EFC.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProjectNameUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "i_x_projects_name",
                table: "projects");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "team_user",
                type: "varchar(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "team_user",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "projects",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.CreateTable(
                name: "budget_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    project_id = table.Column<int>(type: "int", nullable: false),
                    details = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    assigned_budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    executed_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_budget_items", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "machinery_assignments",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(255)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    project_id = table.Column<string>(type: "longtext", nullable: false),
                    machinery_id = table.Column<string>(type: "longtext", nullable: false),
                    name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    assigned_to = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    assigned_worker_id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    registration_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    maintenance_hours = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    assignment_detail = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_machinery_assignments", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<string>(type: "longtext", nullable: false),
                    message = table.Column<string>(type: "longtext", nullable: false),
                    type = table.Column<string>(type: "longtext", nullable: false),
                    related_project_id = table.Column<string>(type: "longtext", nullable: false),
                    payload = table.Column<string>(type: "longtext", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    is_read = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_notifications", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "progress_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    project_id = table.Column<int>(type: "int", nullable: false),
                    planned_percentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    actual_percentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_progress_items", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "activity_names",
                columns: table => new
                {
                    BudgetItemId = table.Column<int>(type: "int", nullable: false),
                    activity_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity_names", x => x.BudgetItemId);
                    table.ForeignKey(
                        name: "FK_activity_names_budget_items_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "budget_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "budget_transactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    budget_item_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_budget_transactions", x => x.id);
                    table.ForeignKey(
                        name: "f_k_budget_transactions_budget_items_budget_item_id",
                        column: x => x.budget_item_id,
                        principalTable: "budget_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "task_names",
                columns: table => new
                {
                    ProgressItemId = table.Column<int>(type: "int", nullable: false),
                    task_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_names", x => x.ProgressItemId);
                    table.ForeignKey(
                        name: "FK_task_names_progress_items_ProgressItemId",
                        column: x => x.ProgressItemId,
                        principalTable: "progress_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_budget_transactions_budget_item_id",
                table: "budget_transactions",
                column: "budget_item_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity_names");

            migrationBuilder.DropTable(
                name: "budget_transactions");

            migrationBuilder.DropTable(
                name: "machinery_assignments");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "task_names");

            migrationBuilder.DropTable(
                name: "budget_items");

            migrationBuilder.DropTable(
                name: "progress_items");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "team_user");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "team_user",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(36)",
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "projects",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.CreateIndex(
                name: "i_x_projects_name",
                table: "projects",
                column: "name",
                unique: true);
        }
    }
}

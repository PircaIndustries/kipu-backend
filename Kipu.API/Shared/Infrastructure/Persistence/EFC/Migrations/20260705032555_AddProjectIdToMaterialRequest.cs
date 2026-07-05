using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kipu.API.Shared.Infrastructure.Persistence.EFC.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectIdToMaterialRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_material_request_items_material_requests_material_request_id1",
                table: "material_request_items");

            migrationBuilder.DropPrimaryKey(
                name: "pk_ncrs",
                table: "ncrs");

            migrationBuilder.DropIndex(
                name: "i_x_material_request_items_material_request_id1",
                table: "material_request_items");

            migrationBuilder.DropColumn(
                name: "material_request_id1",
                table: "material_request_items");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "ncrs",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "severity",
                table: "ncrs",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "ncrs",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "material_requests",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "p_k_ncrs",
                table: "ncrs",
                column: "id");

            migrationBuilder.CreateTable(
                name: "machinery_catalog",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    brand = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    model = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    serial_number = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    acquisition_date = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_machinery_catalogs", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_machinery_catalog_serial_number",
                table: "machinery_catalog",
                column: "serial_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "machinery_catalog");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_ncrs",
                table: "ncrs");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "material_requests");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "ncrs",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "severity",
                table: "ncrs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "ncrs",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "material_request_id1",
                table: "material_request_items",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_ncrs",
                table: "ncrs",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "i_x_material_request_items_material_request_id1",
                table: "material_request_items",
                column: "material_request_id1");

            migrationBuilder.AddForeignKey(
                name: "f_k_material_request_items_material_requests_material_request_id1",
                table: "material_request_items",
                column: "material_request_id1",
                principalTable: "material_requests",
                principalColumn: "id");
        }
    }
}

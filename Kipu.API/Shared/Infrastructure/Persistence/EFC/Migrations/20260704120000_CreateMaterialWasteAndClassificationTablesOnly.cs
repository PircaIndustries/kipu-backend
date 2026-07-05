using Microsoft.EntityFrameworkCore.Migrations;

namespace Kipu.API.Shared.Infrastructure.Persistence.EFC.Migrations;

public partial class CreateMaterialWasteAndClassificationTablesOnly : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // 1. Crear tabla de Clasificaciones
        migrationBuilder.CreateTable(
            name: "waste_classifications",
            columns: table => new
            {
                id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_waste_classifications", x => x.id);
            });

        // 2. Crear tabla de Mermas de Materiales
        migrationBuilder.CreateTable(
            name: "material_wastes",
            columns: table => new
            {
                id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                project_id = table.Column<string>(type: "varchar(255)", nullable: false),
                material_id = table.Column<string>(type: "varchar(255)", nullable: false),
                quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                classification_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                date = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                reported_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                photo_url = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_material_wastes", x => x.id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "material_wastes");
        migrationBuilder.DropTable(name: "waste_classifications");
    }
}
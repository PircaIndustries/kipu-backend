using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Kipu.API.Shared.Infrastructure.Persistence.EFC.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamAndDocumentContexts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "document",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(255)", nullable: false),
                    type = table.Column<string>(type: "longtext", nullable: false),
                    is_signed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    digital_signature_token = table.Column<string>(type: "longtext", nullable: true),
                    deadline = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    project_id = table.Column<string>(type: "longtext", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "material_requests",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    deadline = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    request_status = table.Column<int>(type: "int", nullable: false),
                    request_priority = table.Column<int>(type: "int", nullable: false),
                    delivery_location = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    budget_line_id = table.Column<int>(type: "int", nullable: false),
                    purpose = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    additional_notes = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    requested_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_material_requests", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "team_user",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(255)", nullable: false),
                    full_name = table.Column<string>(type: "longtext", nullable: false),
                    email = table.Column<string>(type: "longtext", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    role = table.Column<string>(type: "longtext", nullable: false),
                    project_id = table.Column<string>(type: "longtext", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team_user", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "team_worker",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(255)", nullable: false),
                    dni = table.Column<string>(type: "longtext", nullable: false),
                    full_name = table.Column<string>(type: "longtext", nullable: false),
                    role = table.Column<string>(type: "longtext", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    project_id = table.Column<string>(type: "longtext", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team_worker", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "documentXteam_user",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    id_team_user = table.Column<string>(type: "longtext", nullable: false),
                    full_name = table.Column<string>(type: "longtext", nullable: false),
                    signed_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    id_document = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentXteam_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_documentXteam_user_document_id_document",
                        column: x => x.id_document,
                        principalTable: "document",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "material_request_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    material_request_id = table.Column<int>(type: "int", nullable: false),
                    material_request_id1 = table.Column<int>(type: "int", nullable: true),
                    material_catalog_id = table.Column<int>(type: "int", nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_material_request_items", x => x.id);
                    table.ForeignKey(
                        name: "f_k_material_request_items_material_requests_material_request_id",
                        column: x => x.material_request_id,
                        principalTable: "material_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_matreq_items_req_id1",
                        column: x => x.material_request_id1,
                        principalTable: "material_requests",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "team_workerXmachinery",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    id_machinery = table.Column<string>(type: "longtext", nullable: false),
                    fullName = table.Column<string>(type: "longtext", nullable: false),
                    id_team_worker = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team_workerXmachinery", x => x.id);
                    table.ForeignKey(
                        name: "FK_team_workerXmachinery_team_worker_id_team_worker",
                        column: x => x.id_team_worker,
                        principalTable: "team_worker",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_documentXteam_user_id_document",
                table: "documentXteam_user",
                column: "id_document");

            migrationBuilder.CreateIndex(
                name: "i_x_material_request_items_material_request_id",
                table: "material_request_items",
                column: "material_request_id");

            migrationBuilder.CreateIndex(
                name: "i_x_material_request_items_material_request_id1",
                table: "material_request_items",
                column: "material_request_id1");

            migrationBuilder.CreateIndex(
                name: "i_x_material_requests_request_status",
                table: "material_requests",
                column: "request_status");

            migrationBuilder.CreateIndex(
                name: "IX_team_workerXmachinery_id_team_worker",
                table: "team_workerXmachinery",
                column: "id_team_worker");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "documentXteam_user");

            migrationBuilder.DropTable(
                name: "material_request_items");

            migrationBuilder.DropTable(
                name: "team_user");

            migrationBuilder.DropTable(
                name: "team_workerXmachinery");

            migrationBuilder.DropTable(
                name: "document");

            migrationBuilder.DropTable(
                name: "material_requests");

            migrationBuilder.DropTable(
                name: "team_worker");
        }
    }
}

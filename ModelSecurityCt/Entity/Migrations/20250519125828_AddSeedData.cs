using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "form",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "module",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_module", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateBorn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Eps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedPerson = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "formModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_formModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_formModule_form_FormId",
                        column: x => x.FormId,
                        principalTable: "form",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_formModule_module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rolFormPermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rolFormPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rolFormPermission_form_FormId",
                        column: x => x.FormId,
                        principalTable: "form",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rolFormPermission_permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rolFormPermission_rol_RolId",
                        column: x => x.RolId,
                        principalTable: "rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rolUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rolUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rolUser_rol_RolId",
                        column: x => x.RolId,
                        principalTable: "rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rolUser_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "form",
                columns: new[] { "Id", "Description", "IsDeleted", "Name", "Url" },
                values: new object[,]
                {
                    { 1, "Formulario principal del sistema", false, "Formulario Principal", "/dashboard" },
                    { 2, "Formulario para administración de usuarios", false, "Gestión de Usuarios", "/usuarios" },
                    { 3, "Formulario para visualizar reportes", false, "Reportes", "/reportes" }
                });

            migrationBuilder.InsertData(
                table: "module",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, "Módulo de administración general", false, "Administración" },
                    { 2, "Control y mantenimiento de usuarios", false, "Gestión de usuarios" },
                    { 3, "Visualización y exportación de reportes", false, "Reportes" }
                });

            migrationBuilder.InsertData(
                table: "permission",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, "Permite crear registros", false, "Crear" },
                    { 2, "Permite editar registros", false, "Editar" },
                    { 3, "Permite eliminar registros", false, "Eliminar" },
                    { 4, "Permite ver registros", false, "Ver" }
                });

            migrationBuilder.InsertData(
                table: "person",
                columns: new[] { "Id", "DateBorn", "Document", "DocumentType", "Eps", "FirstName", "Genero", "LastName", "PhoneNumber", "RelatedPerson" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "1075455", "TI", "Nueva EPS", "Marcos", "Masculino", "Rojas Alvarez", "30012345", false },
                    { 2, new DateTime(1965, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "121243333", "CC", "Sura", "Gentil", "Masculino", "Rojas Cortes", "30198763", true }
                });

            migrationBuilder.InsertData(
                table: "rol",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Rol con todos los permisos del sistema", "Administrador" },
                    { 2, "Rol con permisos limitados", "Usuario" }
                });

            migrationBuilder.InsertData(
                table: "formModule",
                columns: new[] { "Id", "FormId", "IsDeleted", "ModuleId" },
                values: new object[,]
                {
                    { 1, 1, false, 1 },
                    { 2, 2, false, 1 },
                    { 3, 2, false, 2 }
                });

            migrationBuilder.InsertData(
                table: "rolFormPermission",
                columns: new[] { "Id", "FormId", "IsDeleted", "PermissionId", "RolId" },
                values: new object[,]
                {
                    { 1, 1, false, 1, 1 },
                    { 2, 1, false, 2, 1 },
                    { 3, 2, false, 4, 2 }
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "Id", "Active", "Email", "Password", "PersonId" },
                values: new object[,]
                {
                    { 1, true, "marcosrojasalvarez09172007@gmail.com", "1234", 1 },
                    { 2, true, "gentilrojas@gmail.com", "123", 2 }
                });

            migrationBuilder.InsertData(
                table: "rolUser",
                columns: new[] { "Id", "Email", "RolId", "UserId" },
                values: new object[,]
                {
                    { 1, "marcosrojasalvarez09172007@gmail.com", 1, 1 },
                    { 2, "gentilrojas@gmail.com", 2, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_formModule_FormId",
                table: "formModule",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_formModule_ModuleId",
                table: "formModule",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_rolFormPermission_FormId",
                table: "rolFormPermission",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_rolFormPermission_PermissionId",
                table: "rolFormPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_rolFormPermission_RolId",
                table: "rolFormPermission",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_rolUser_RolId",
                table: "rolUser",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_rolUser_UserId",
                table: "rolUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_PersonId",
                table: "user",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "formModule");

            migrationBuilder.DropTable(
                name: "rolFormPermission");

            migrationBuilder.DropTable(
                name: "rolUser");

            migrationBuilder.DropTable(
                name: "module");

            migrationBuilder.DropTable(
                name: "form");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "person");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PruebaViamatica.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    IdPersona = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "varchar(60)", nullable: false),
                    Apellidos = table.Column<string>(type: "varchar(60)", nullable: false),
                    Identificacion = table.Column<string>(type: "varchar(10)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.IdPersona);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolName = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UseName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Password = table.Column<string>(type: "varchar(50)", nullable: false),
                    Mail = table.Column<string>(type: "varchar(120)", nullable: false),
                    SessionActive = table.Column<string>(type: "varchar(1)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false),
                    PersonaIdPersona2 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Personas_PersonaIdPersona2",
                        column: x => x.PersonaIdPersona2,
                        principalTable: "Personas",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolesOpciones",
                columns: table => new
                {
                    RolIdRol = table.Column<int>(type: "int", nullable: false),
                    RolOpcionesIdOpcion = table.Column<int>(type: "int", nullable: false),
                    RolOpcionNavigationRolIdRol = table.Column<int>(type: "int", nullable: false),
                    RolOpcionNavigationRolOpcionesIdOpcion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesOpciones", x => new { x.RolIdRol, x.RolOpcionesIdOpcion });
                    table.ForeignKey(
                        name: "FK_RolesOpciones_RolesOpciones_RolOpcionNavigationRolIdRol_RolOpcionNavigationRolOpcionesIdOpcion",
                        columns: x => new { x.RolOpcionNavigationRolIdRol, x.RolOpcionNavigationRolOpcionesIdOpcion },
                        principalTable: "RolesOpciones",
                        principalColumns: new[] { "RolIdRol", "RolOpcionesIdOpcion" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolesOpciones_Roles_RolIdRol",
                        column: x => x.RolIdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolesUsuarios",
                columns: table => new
                {
                    RolIdRol = table.Column<int>(type: "int", nullable: false),
                    UsuariosIdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesUsuarios", x => new { x.RolIdRol, x.UsuariosIdUsuario });
                    table.ForeignKey(
                        name: "FK_RolesUsuarios_Roles_RolIdRol",
                        column: x => x.RolIdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolesUsuarios_Usuarios_UsuariosIdUsuario",
                        column: x => x.UsuariosIdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuariosIdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Usuarios_UsuariosIdUsuario",
                        column: x => x.UsuariosIdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolesOpciones_RolOpcionNavigationRolIdRol_RolOpcionNavigationRolOpcionesIdOpcion",
                table: "RolesOpciones",
                columns: new[] { "RolOpcionNavigationRolIdRol", "RolOpcionNavigationRolOpcionesIdOpcion" });

            migrationBuilder.CreateIndex(
                name: "IX_RolesUsuarios_UsuariosIdUsuario",
                table: "RolesUsuarios",
                column: "UsuariosIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UsuariosIdUsuario",
                table: "Sessions",
                column: "UsuariosIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Mail",
                table: "Usuarios",
                column: "Mail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PersonaIdPersona2",
                table: "Usuarios",
                column: "PersonaIdPersona2");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_UseName",
                table: "Usuarios",
                column: "UseName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolesOpciones");

            migrationBuilder.DropTable(
                name: "RolesUsuarios");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Personas");
        }
    }
}

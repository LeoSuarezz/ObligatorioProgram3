using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObligatorioProgram3.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsuarioRolRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolPermiso_Rols_IdRol",
                table: "RolPermiso");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosRoles",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rols",
                table: "Rols");

            migrationBuilder.RenameTable(
                name: "Rols",
                newName: "Rol");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rol",
                table: "Rol",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolPermiso_Rol_IdRol",
                table: "RolPermiso",
                column: "IdRol",
                principalTable: "Rol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosRoles",
                table: "Usuarios",
                column: "IDRol",
                principalTable: "Rol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolPermiso_Rol_IdRol",
                table: "RolPermiso");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosRoles",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rol",
                table: "Rol");

            migrationBuilder.RenameTable(
                name: "Rol",
                newName: "Rols");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rols",
                table: "Rols",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolPermiso_Rols_IdRol",
                table: "RolPermiso",
                column: "IdRol",
                principalTable: "Rols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosRoles",
                table: "Usuarios",
                column: "IDRol",
                principalTable: "Rols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

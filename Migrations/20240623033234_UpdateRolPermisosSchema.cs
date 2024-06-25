using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObligatorioProgram3.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRolPermisosSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Apellido = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    TipoCliente = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clientes__3214EC27AAE9DA96", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Clima",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Temperatura = table.Column<decimal>(type: "decimal(2,2)", nullable: false),
                    DescripcionClima = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clima__3214EC27180A4653", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Cotizacion",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Moneda = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cotizaci__3214EC27815FD456", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombrePlato = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    RutaImagen = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Menu__3214EC278613F335", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurantes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Direccion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Restaura__3214EC27D6EF9985", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mesas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroMesa = table.Column<int>(type: "int", nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    IDRestaurante = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Mesas__3214EC27EEDBF2F1", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MesaRestaurante",
                        column: x => x.IDRestaurante,
                        principalTable: "Restaurantes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reseñas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Puntaje = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    FechaReseña = table.Column<DateOnly>(type: "date", nullable: false),
                    IDCliente = table.Column<int>(type: "int", nullable: true),
                    IDRestaurante = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reseñas__3214EC277E3C5AC0", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReseñaCliente",
                        column: x => x.IDCliente,
                        principalTable: "Clientes",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ReseñaRestaurante",
                        column: x => x.IDRestaurante,
                        principalTable: "Restaurantes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RolPermiso",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    IdPermisos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolPermiso", x => new { x.IdRol, x.IdPermisos });
                    table.ForeignKey(
                        name: "FK_RolPermiso_Permisos_IdPermisos",
                        column: x => x.IdPermisos,
                        principalTable: "Permisos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolPermiso_Rols_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Rols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDRol = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Apellido = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Contraseña = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__3214EC2707E35BEF", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UsuariosRoles",
                        column: x => x.IDRol,
                        principalTable: "Rols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDCliente = table.Column<int>(type: "int", nullable: true),
                    IDMesa = table.Column<int>(type: "int", nullable: true),
                    FechaReserva = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reservas__3214EC271470247F", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReservaClientes",
                        column: x => x.IDCliente,
                        principalTable: "Clientes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservaMesas",
                        column: x => x.IDMesa,
                        principalTable: "Mesas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ordenes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDReserva = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ordenes__3214EC2760805297", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrdenReserva",
                        column: x => x.IDReserva,
                        principalTable: "Reservas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDReserva = table.Column<int>(type: "int", nullable: true),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FechaPago = table.Column<DateOnly>(type: "date", nullable: false),
                    IDCotizacion = table.Column<int>(type: "int", nullable: true),
                    MetodoPago = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    IDClima = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pagos__3214EC27A3E14295", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PagosClima",
                        column: x => x.IDClima,
                        principalTable: "Clima",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PagosCotizacion",
                        column: x => x.IDCotizacion,
                        principalTable: "Cotizacion",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PagosReserva",
                        column: x => x.IDReserva,
                        principalTable: "Reservas",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "OrdenDetalle",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    IDOrden = table.Column<int>(type: "int", nullable: true),
                    IDMenu = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrdenDet__3214EC27293E9D4B", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DetalleOrden",
                        column: x => x.IDOrden,
                        principalTable: "Ordenes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenMenu",
                        column: x => x.IDMenu,
                        principalTable: "Menu",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mesas_IDRestaurante",
                table: "Mesas",
                column: "IDRestaurante");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenDetalle_IDMenu",
                table: "OrdenDetalle",
                column: "IDMenu");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenDetalle_IDOrden",
                table: "OrdenDetalle",
                column: "IDOrden");

            migrationBuilder.CreateIndex(
                name: "IX_Ordenes_IDReserva",
                table: "Ordenes",
                column: "IDReserva");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IDClima",
                table: "Pagos",
                column: "IDClima");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IDCotizacion",
                table: "Pagos",
                column: "IDCotizacion");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IDReserva",
                table: "Pagos",
                column: "IDReserva");

            migrationBuilder.CreateIndex(
                name: "IX_Reseñas_IDCliente",
                table: "Reseñas",
                column: "IDCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Reseñas_IDRestaurante",
                table: "Reseñas",
                column: "IDRestaurante");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IDCliente",
                table: "Reservas",
                column: "IDCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IDMesa",
                table: "Reservas",
                column: "IDMesa");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_IdPermisos",
                table: "RolPermiso",
                column: "IdPermisos");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IDRol",
                table: "Usuarios",
                column: "IDRol");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__A9D10534CAEBD9BC",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdenDetalle");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Reseñas");

            migrationBuilder.DropTable(
                name: "RolPermiso");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Ordenes");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Clima");

            migrationBuilder.DropTable(
                name: "Cotizacion");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Rols");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Mesas");

            migrationBuilder.DropTable(
                name: "Restaurantes");
        }
    }
}

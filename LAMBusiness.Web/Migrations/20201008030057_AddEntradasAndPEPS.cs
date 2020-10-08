using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LAMBusiness.Web.Migrations
{
    public partial class AddEntradasAndPEPS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Colaboradores",
                keyColumn: "ColaboradorID",
                keyValue: new Guid("9a66ba26-eecc-46f0-9855-59a1585d5a1c"));

            migrationBuilder.DeleteData(
                table: "Existencias",
                keyColumn: "ExistenciaID",
                keyValue: new Guid("1f390971-8f4a-4803-b45e-fec53d9ee48b"));

            migrationBuilder.DeleteData(
                table: "Existencias",
                keyColumn: "ExistenciaID",
                keyValue: new Guid("7a433664-5513-48d7-b032-7e69e9a92016"));

            migrationBuilder.CreateTable(
                name: "Entradas",
                columns: table => new
                {
                    EntradaID = table.Column<Guid>(nullable: false),
                    ProveedorID = table.Column<Guid>(nullable: false),
                    UsuarioID = table.Column<Guid>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Folio = table.Column<string>(maxLength: 10, nullable: false),
                    Observaciones = table.Column<string>(nullable: true),
                    Aplicado = table.Column<bool>(nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaActualizacion = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entradas", x => x.EntradaID);
                    table.ForeignKey(
                        name: "FK_Entradas_Proveedores_ProveedorID",
                        column: x => x.ProveedorID,
                        principalTable: "Proveedores",
                        principalColumn: "ProveedorID");
                });

            migrationBuilder.CreateTable(
                name: "PrimerasEntradasPrimerasSalidas",
                columns: table => new
                {
                    PEPSID = table.Column<Guid>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    ProductoID = table.Column<Guid>(nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    PrecioCosto = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimerasEntradasPrimerasSalidas", x => x.PEPSID);
                    table.ForeignKey(
                        name: "FK_PrimerasEntradasPrimerasSalidas_Productos_ProductoID",
                        column: x => x.ProductoID,
                        principalTable: "Productos",
                        principalColumn: "ProductoID");
                });

            migrationBuilder.CreateTable(
                name: "EntradasDetalle",
                columns: table => new
                {
                    EntradaDetalleID = table.Column<Guid>(nullable: false),
                    EntradaID = table.Column<Guid>(nullable: false),
                    ProductoID = table.Column<Guid>(nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    PrecioCosto = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradasDetalle", x => x.EntradaDetalleID);
                    table.ForeignKey(
                        name: "FK_EntradasDetalle_Entradas_EntradaID",
                        column: x => x.EntradaID,
                        principalTable: "Entradas",
                        principalColumn: "EntradaID");
                    table.ForeignKey(
                        name: "FK_EntradasDetalle_Productos_ProductoID",
                        column: x => x.ProductoID,
                        principalTable: "Productos",
                        principalColumn: "ProductoID");
                });

            migrationBuilder.InsertData(
                table: "Colaboradores",
                columns: new[] { "ColaboradorID", "Activo", "CURP", "CodigoPostal", "Colonia", "Domicilio", "Email", "EstadoCivilID", "EstadoNacimientoID", "FechaNacimiento", "FechaRegistro", "GeneroID", "MunicipioID", "Nombre", "PrimerApellido", "PuestoID", "SegundoApellido", "Telefono", "TelefonoMovil" },
                values: new object[] { new Guid("9a5a1084-1f5b-4a5d-9061-ea9392b61506"), true, "CURP781227HCSRNS00", 29000, "COLONIA", "DOMICILIO", "administrador@lambusiness.com", (short)2, (short)7, new DateTime(1978, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 7, 22, 0, 55, 896, DateTimeKind.Local).AddTicks(1226), "M", 180, "NOMBRE", "PRIMERAPELLIDO", new Guid("d6d5973b-fa59-4b0f-837a-35f83350a63e"), "SEGUNDOAPELLIDO", "1234567890", "0123456789" });

            migrationBuilder.InsertData(
                table: "Existencias",
                columns: new[] { "ExistenciaID", "AlmacenID", "ExistenciaEnAlmacen", "ProductoID" },
                values: new object[] { new Guid("9207b250-1e63-4f2e-8fb0-2a385aec8e5d"), new Guid("8706ef28-2eba-463a-bab4-62227965f03f"), 0m, new Guid("de7c7462-69ba-4343-a328-012f48f013af") });

            migrationBuilder.InsertData(
                table: "Existencias",
                columns: new[] { "ExistenciaID", "AlmacenID", "ExistenciaEnAlmacen", "ProductoID" },
                values: new object[] { new Guid("06327217-032e-4b4b-8971-971eaadaf39c"), new Guid("8706ef28-2eba-463a-bab4-62227965f03f"), 0m, new Guid("38abf163-90ad-4d67-9bab-e5867d2715cf") });

            migrationBuilder.CreateIndex(
                name: "IX_Entradas_ProveedorID",
                table: "Entradas",
                column: "ProveedorID");

            migrationBuilder.CreateIndex(
                name: "IX_EntradasDetalle_EntradaID",
                table: "EntradasDetalle",
                column: "EntradaID");

            migrationBuilder.CreateIndex(
                name: "IX_EntradasDetalle_ProductoID",
                table: "EntradasDetalle",
                column: "ProductoID");

            migrationBuilder.CreateIndex(
                name: "IX_PrimerasEntradasPrimerasSalidas_ProductoID",
                table: "PrimerasEntradasPrimerasSalidas",
                column: "ProductoID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntradasDetalle");

            migrationBuilder.DropTable(
                name: "PrimerasEntradasPrimerasSalidas");

            migrationBuilder.DropTable(
                name: "Entradas");

            migrationBuilder.DeleteData(
                table: "Colaboradores",
                keyColumn: "ColaboradorID",
                keyValue: new Guid("9a5a1084-1f5b-4a5d-9061-ea9392b61506"));

            migrationBuilder.DeleteData(
                table: "Existencias",
                keyColumn: "ExistenciaID",
                keyValue: new Guid("06327217-032e-4b4b-8971-971eaadaf39c"));

            migrationBuilder.DeleteData(
                table: "Existencias",
                keyColumn: "ExistenciaID",
                keyValue: new Guid("9207b250-1e63-4f2e-8fb0-2a385aec8e5d"));

            migrationBuilder.InsertData(
                table: "Colaboradores",
                columns: new[] { "ColaboradorID", "Activo", "CURP", "CodigoPostal", "Colonia", "Domicilio", "Email", "EstadoCivilID", "EstadoNacimientoID", "FechaNacimiento", "FechaRegistro", "GeneroID", "MunicipioID", "Nombre", "PrimerApellido", "PuestoID", "SegundoApellido", "Telefono", "TelefonoMovil" },
                values: new object[] { new Guid("9a66ba26-eecc-46f0-9855-59a1585d5a1c"), true, "CURP781227HCSRNS00", 29000, "COLONIA", "DOMICILIO", "administrador@lambusiness.com", (short)2, (short)7, new DateTime(1978, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 7, 21, 53, 29, 626, DateTimeKind.Local).AddTicks(5475), "M", 180, "NOMBRE", "PRIMERAPELLIDO", new Guid("d6d5973b-fa59-4b0f-837a-35f83350a63e"), "SEGUNDOAPELLIDO", "1234567890", "0123456789" });

            migrationBuilder.InsertData(
                table: "Existencias",
                columns: new[] { "ExistenciaID", "AlmacenID", "ExistenciaEnAlmacen", "ProductoID" },
                values: new object[] { new Guid("1f390971-8f4a-4803-b45e-fec53d9ee48b"), new Guid("8706ef28-2eba-463a-bab4-62227965f03f"), 0m, new Guid("de7c7462-69ba-4343-a328-012f48f013af") });

            migrationBuilder.InsertData(
                table: "Existencias",
                columns: new[] { "ExistenciaID", "AlmacenID", "ExistenciaEnAlmacen", "ProductoID" },
                values: new object[] { new Guid("7a433664-5513-48d7-b032-7e69e9a92016"), new Guid("8706ef28-2eba-463a-bab4-62227965f03f"), 0m, new Guid("38abf163-90ad-4d67-9bab-e5867d2715cf") });
        }
    }
}

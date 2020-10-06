using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LAMBusiness.Web.Migrations
{
    public partial class AddPaqueteConfiguracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Colaboradores",
                keyColumn: "ColaboradorID",
                keyValue: new Guid("61ef6c63-4fdf-4dc1-89c9-56d2ad6f9f3a"));

            migrationBuilder.DeleteData(
                table: "Existencias",
                keyColumn: "ExistenciaID",
                keyValue: new Guid("426676e0-b835-440d-9b9e-c03a6251d3a7"));

            migrationBuilder.DeleteData(
                table: "Existencias",
                keyColumn: "ExistenciaID",
                keyValue: new Guid("f421f650-5e04-4d18-843e-2e7b4f12f4a6"));

            migrationBuilder.InsertData(
                table: "Colaboradores",
                columns: new[] { "ColaboradorID", "Activo", "CURP", "CodigoPostal", "Colonia", "Domicilio", "Email", "EstadoCivilID", "EstadoNacimientoID", "FechaNacimiento", "FechaRegistro", "GeneroID", "MunicipioID", "Nombre", "PrimerApellido", "PuestoID", "SegundoApellido", "Telefono", "TelefonoMovil" },
                values: new object[] { new Guid("a81256ef-ba5c-4449-97eb-fdb9fd4c3516"), true, "CURP781227HCSRNS00", 29000, "COLONIA", "DOMICILIO", "administrador@lambusiness.com", (short)2, (short)7, new DateTime(1978, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 4, 9, 24, 24, 919, DateTimeKind.Local).AddTicks(9430), "M", 180, "NOMBRE", "PRIMERAPELLIDO", new Guid("d6d5973b-fa59-4b0f-837a-35f83350a63e"), "SEGUNDOAPELLIDO", "1234567890", "0123456789" });

            migrationBuilder.InsertData(
                table: "Existencias",
                columns: new[] { "ExistenciaID", "AlmacenID", "ExistenciaEnAlmacen", "ExistenciaEnAlmacenMaxima", "ExistenciaEnAlmacenMinima", "ProductoID" },
                values: new object[,]
                {
                    { new Guid("962b837a-e817-42e1-8976-121592111ec0"), new Guid("8706ef28-2eba-463a-bab4-62227965f03f"), 22m, 30m, 12m, new Guid("de7c7462-69ba-4343-a328-012f48f013af") },
                    { new Guid("360818ed-c30d-4b93-a8a5-1a705b21373b"), new Guid("8706ef28-2eba-463a-bab4-62227965f03f"), 5.5m, 15m, 7m, new Guid("38abf163-90ad-4d67-9bab-e5867d2715cf") }
                });

            migrationBuilder.InsertData(
                table: "Paquetes",
                columns: new[] { "ProductoID", "CantidadProductoxPaquete", "PiezaProductoID" },
                values: new object[,]
                {
                    { new Guid("94c079ee-1fbe-4cae-9a16-443261dd0d60"), 12m, new Guid("de7c7462-69ba-4343-a328-012f48f013af") },
                    { new Guid("435a7b4d-1347-4282-9b06-3792ed1a99c4"), 20m, new Guid("38abf163-90ad-4d67-9bab-e5867d2715cf") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Colaboradores",
                keyColumn: "ColaboradorID",
                keyValue: new Guid("a81256ef-ba5c-4449-97eb-fdb9fd4c3516"));

            migrationBuilder.DeleteData(
                table: "Existencias",
                keyColumn: "ExistenciaID",
                keyValue: new Guid("360818ed-c30d-4b93-a8a5-1a705b21373b"));

            migrationBuilder.DeleteData(
                table: "Existencias",
                keyColumn: "ExistenciaID",
                keyValue: new Guid("962b837a-e817-42e1-8976-121592111ec0"));

            migrationBuilder.DeleteData(
                table: "Paquetes",
                keyColumn: "ProductoID",
                keyValue: new Guid("435a7b4d-1347-4282-9b06-3792ed1a99c4"));

            migrationBuilder.DeleteData(
                table: "Paquetes",
                keyColumn: "ProductoID",
                keyValue: new Guid("94c079ee-1fbe-4cae-9a16-443261dd0d60"));

            migrationBuilder.InsertData(
                table: "Colaboradores",
                columns: new[] { "ColaboradorID", "Activo", "CURP", "CodigoPostal", "Colonia", "Domicilio", "Email", "EstadoCivilID", "EstadoNacimientoID", "FechaNacimiento", "FechaRegistro", "GeneroID", "MunicipioID", "Nombre", "PrimerApellido", "PuestoID", "SegundoApellido", "Telefono", "TelefonoMovil" },
                values: new object[] { new Guid("61ef6c63-4fdf-4dc1-89c9-56d2ad6f9f3a"), true, "CURP781227HCSRNS00", 29000, "COLONIA", "DOMICILIO", "administrador@lambusiness.com", (short)2, (short)7, new DateTime(1978, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 4, 9, 16, 17, 738, DateTimeKind.Local).AddTicks(1663), "M", 180, "NOMBRE", "PRIMERAPELLIDO", new Guid("d6d5973b-fa59-4b0f-837a-35f83350a63e"), "SEGUNDOAPELLIDO", "1234567890", "0123456789" });

            migrationBuilder.InsertData(
                table: "Existencias",
                columns: new[] { "ExistenciaID", "AlmacenID", "ExistenciaEnAlmacen", "ExistenciaEnAlmacenMaxima", "ExistenciaEnAlmacenMinima", "ProductoID" },
                values: new object[] { new Guid("426676e0-b835-440d-9b9e-c03a6251d3a7"), new Guid("8706ef28-2eba-463a-bab4-62227965f03f"), 22m, 30m, 12m, new Guid("de7c7462-69ba-4343-a328-012f48f013af") });

            migrationBuilder.InsertData(
                table: "Existencias",
                columns: new[] { "ExistenciaID", "AlmacenID", "ExistenciaEnAlmacen", "ExistenciaEnAlmacenMaxima", "ExistenciaEnAlmacenMinima", "ProductoID" },
                values: new object[] { new Guid("f421f650-5e04-4d18-843e-2e7b4f12f4a6"), new Guid("8706ef28-2eba-463a-bab4-62227965f03f"), 5.5m, 15m, 7m, new Guid("38abf163-90ad-4d67-9bab-e5867d2715cf") });
        }
    }
}

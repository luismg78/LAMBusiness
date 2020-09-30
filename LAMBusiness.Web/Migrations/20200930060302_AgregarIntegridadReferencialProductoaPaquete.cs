using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LAMBusiness.Web.Migrations
{
    public partial class AgregarIntegridadReferencialProductoaPaquete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Paquetes_PaqueteProductoID",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_PaqueteProductoID",
                table: "Productos");

            migrationBuilder.DeleteData(
                table: "Colaboradores",
                keyColumn: "ColaboradorID",
                keyValue: new Guid("57c0de81-2adb-4fd5-ad7b-117b33f4b5c7"));

            migrationBuilder.DropColumn(
                name: "PaqueteProductoID",
                table: "Productos");

            migrationBuilder.InsertData(
                table: "Colaboradores",
                columns: new[] { "ColaboradorID", "Activo", "CURP", "CodigoPostal", "Colonia", "Domicilio", "Email", "EstadoCivilID", "EstadoNacimientoID", "FechaNacimiento", "FechaRegistro", "GeneroID", "MunicipioID", "Nombre", "PrimerApellido", "PuestoID", "SegundoApellido", "Telefono", "TelefonoMovil" },
                values: new object[] { new Guid("681a1052-9b5c-4d25-b31b-599a4714c3aa"), true, "CURP781227HCSRNS00", 29000, "COLONIA", "DOMICILIO", "administrador@lambusiness.com", (short)2, (short)7, new DateTime(1978, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 9, 30, 1, 3, 0, 68, DateTimeKind.Local).AddTicks(5252), "M", 180, "NOMBRE", "PRIMERAPELLIDO", new Guid("d6d5973b-fa59-4b0f-837a-35f83350a63e"), "SEGUNDOAPELLIDO", "1234567890", "0123456789" });

            migrationBuilder.AddForeignKey(
                name: "FK_Paquetes_Productos_ProductoID",
                table: "Paquetes",
                column: "ProductoID",
                principalTable: "Productos",
                principalColumn: "ProductoID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paquetes_Productos_ProductoID",
                table: "Paquetes");

            migrationBuilder.DeleteData(
                table: "Colaboradores",
                keyColumn: "ColaboradorID",
                keyValue: new Guid("681a1052-9b5c-4d25-b31b-599a4714c3aa"));

            migrationBuilder.AddColumn<Guid>(
                name: "PaqueteProductoID",
                table: "Productos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Colaboradores",
                columns: new[] { "ColaboradorID", "Activo", "CURP", "CodigoPostal", "Colonia", "Domicilio", "Email", "EstadoCivilID", "EstadoNacimientoID", "FechaNacimiento", "FechaRegistro", "GeneroID", "MunicipioID", "Nombre", "PrimerApellido", "PuestoID", "SegundoApellido", "Telefono", "TelefonoMovil" },
                values: new object[] { new Guid("57c0de81-2adb-4fd5-ad7b-117b33f4b5c7"), true, "CURP781227HCSRNS00", 29000, "COLONIA", "DOMICILIO", "administrador@lambusiness.com", (short)2, (short)7, new DateTime(1978, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 9, 30, 0, 54, 22, 180, DateTimeKind.Local).AddTicks(9006), "M", 180, "NOMBRE", "PRIMERAPELLIDO", new Guid("d6d5973b-fa59-4b0f-837a-35f83350a63e"), "SEGUNDOAPELLIDO", "1234567890", "0123456789" });

            migrationBuilder.CreateIndex(
                name: "IX_Productos_PaqueteProductoID",
                table: "Productos",
                column: "PaqueteProductoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Paquetes_PaqueteProductoID",
                table: "Productos",
                column: "PaqueteProductoID",
                principalTable: "Paquetes",
                principalColumn: "ProductoID");
        }
    }
}

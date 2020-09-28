using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LAMBusiness.Web.Migrations
{
    public partial class CorrecionAtributoForaneoEstadoEnMunicipio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Municipios_Estados_Estado",
                table: "Municipios");

            migrationBuilder.DropIndex(
                name: "IX_Municipios_Estado",
                table: "Municipios");

            migrationBuilder.DeleteData(
                table: "Colaboradores",
                keyColumn: "ColaboradorID",
                keyValue: new Guid("5e15856a-d894-4a85-8ffc-1b0b96b335a6"));

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Municipios");

            migrationBuilder.InsertData(
                table: "Colaboradores",
                columns: new[] { "ColaboradorID", "Activo", "CURP", "CodigoPostal", "Colonia", "Domicilio", "Email", "EstadoCivilID", "EstadoNacimientoID", "FechaNacimiento", "FechaRegistro", "GeneroID", "MunicipioID", "Nombre", "PrimerApellido", "PuestoID", "SegundoApellido", "Telefono", "TelefonoMovil" },
                values: new object[] { new Guid("f33818e6-0abd-436e-8b78-832d7b88440a"), true, "CURP781227HCSRNS00", 29000, "COLONIA", "DOMICILIO", "administrador@lambusiness.com", (short)2, (short)7, new DateTime(1978, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 9, 26, 2, 39, 34, 745, DateTimeKind.Local).AddTicks(9551), "M", 180, "NOMBRE", "PRIMERAPELLIDO", new Guid("d6d5973b-fa59-4b0f-837a-35f83350a63e"), "SEGUNDOAPELLIDO", "1234567890", "0123456789" });

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_EstadoID",
                table: "Municipios",
                column: "EstadoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipios_Estados_EstadoID",
                table: "Municipios",
                column: "EstadoID",
                principalTable: "Estados",
                principalColumn: "EstadoID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Municipios_Estados_EstadoID",
                table: "Municipios");

            migrationBuilder.DropIndex(
                name: "IX_Municipios_EstadoID",
                table: "Municipios");

            migrationBuilder.DeleteData(
                table: "Colaboradores",
                keyColumn: "ColaboradorID",
                keyValue: new Guid("f33818e6-0abd-436e-8b78-832d7b88440a"));

            migrationBuilder.AddColumn<short>(
                name: "Estado",
                table: "Municipios",
                type: "smallint",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Colaboradores",
                columns: new[] { "ColaboradorID", "Activo", "CURP", "CodigoPostal", "Colonia", "Domicilio", "Email", "EstadoCivilID", "EstadoNacimientoID", "FechaNacimiento", "FechaRegistro", "GeneroID", "MunicipioID", "Nombre", "PrimerApellido", "PuestoID", "SegundoApellido", "Telefono", "TelefonoMovil" },
                values: new object[] { new Guid("5e15856a-d894-4a85-8ffc-1b0b96b335a6"), true, "CURP781227HCSRNS00", 29000, "COLONIA", "DOMICILIO", "administrador@lambusiness.com", (short)2, (short)7, new DateTime(1978, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 9, 26, 2, 28, 12, 405, DateTimeKind.Local).AddTicks(2219), "M", 180, "NOMBRE", "PRIMERAPELLIDO", new Guid("d6d5973b-fa59-4b0f-837a-35f83350a63e"), "SEGUNDOAPELLIDO", "1234567890", "0123456789" });

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_Estado",
                table: "Municipios",
                column: "Estado");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipios_Estados_Estado",
                table: "Municipios",
                column: "Estado",
                principalTable: "Estados",
                principalColumn: "EstadoID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

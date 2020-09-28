using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LAMBusiness.Web.Migrations
{
    public partial class AddEntitiesCatalogosAndContactos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    EstadoID = table.Column<short>(nullable: false),
                    EstadoClave = table.Column<string>(maxLength: 5, nullable: false),
                    EstadoDescripcion = table.Column<string>(maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.EstadoID);
                });

            migrationBuilder.CreateTable(
                name: "EstadosCiviles",
                columns: table => new
                {
                    EstadoCivilID = table.Column<short>(nullable: false),
                    EstadoCivilDescripcion = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosCiviles", x => x.EstadoCivilID);
                });

            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    GeneroID = table.Column<string>(maxLength: 1, nullable: false),
                    GeneroDescripcion = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.GeneroID);
                });

            migrationBuilder.CreateTable(
                name: "Puestos",
                columns: table => new
                {
                    PuestoID = table.Column<Guid>(nullable: false),
                    PuestoNombre = table.Column<string>(maxLength: 50, nullable: false),
                    PuestoDescripcion = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puestos", x => x.PuestoID);
                });

            migrationBuilder.CreateTable(
                name: "TasasImpuestos",
                columns: table => new
                {
                    TasaID = table.Column<Guid>(nullable: false),
                    Tasa = table.Column<string>(maxLength: 45, nullable: false),
                    Porcentaje = table.Column<short>(nullable: false),
                    TasaDescripcion = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasasImpuestos", x => x.TasaID);
                });

            migrationBuilder.CreateTable(
                name: "Unidades",
                columns: table => new
                {
                    UnidadID = table.Column<Guid>(nullable: false),
                    UnidadNombre = table.Column<string>(maxLength: 25, nullable: false),
                    UnidadDescripcion = table.Column<string>(maxLength: 150, nullable: false),
                    Pieza = table.Column<bool>(nullable: false),
                    Paquete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidades", x => x.UnidadID);
                });

            migrationBuilder.CreateTable(
                name: "Municipios",
                columns: table => new
                {
                    MunicipioID = table.Column<int>(nullable: false),
                    EstadoID = table.Column<short>(nullable: false),
                    Estado = table.Column<short>(nullable: true),
                    MunicipioClave = table.Column<short>(nullable: false),
                    MunicipioDescripcion = table.Column<string>(maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipios", x => x.MunicipioID);
                    table.ForeignKey(
                        name: "FK_Municipios_Estados_Estado",
                        column: x => x.Estado,
                        principalTable: "Estados",
                        principalColumn: "EstadoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    ProductoID = table.Column<Guid>(nullable: false),
                    Codigo = table.Column<string>(maxLength: 14, nullable: false),
                    ProductoNombre = table.Column<string>(maxLength: 75, nullable: false),
                    ProductoDescripcion = table.Column<string>(maxLength: 75, nullable: false),
                    UnidadID = table.Column<Guid>(nullable: false),
                    TasaID = table.Column<Guid>(nullable: false),
                    Existencia = table.Column<decimal>(nullable: false),
                    PrecioCosto = table.Column<decimal>(nullable: false),
                    PrecioVenta = table.Column<decimal>(nullable: false),
                    ExistenciaMaxima = table.Column<decimal>(nullable: false),
                    Activo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.ProductoID);
                    table.ForeignKey(
                        name: "FK_Productos_TasasImpuestos_TasaID",
                        column: x => x.TasaID,
                        principalTable: "TasasImpuestos",
                        principalColumn: "TasaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Productos_Unidades_UnidadID",
                        column: x => x.UnidadID,
                        principalTable: "Unidades",
                        principalColumn: "UnidadID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteID = table.Column<Guid>(nullable: false),
                    RFC = table.Column<string>(maxLength: 13, nullable: false),
                    Nombre = table.Column<string>(maxLength: 75, nullable: false),
                    Contacto = table.Column<string>(maxLength: 100, nullable: false),
                    Domicilio = table.Column<string>(maxLength: 100, nullable: false),
                    Colonia = table.Column<string>(maxLength: 100, nullable: false),
                    CodigoPostal = table.Column<int>(nullable: false),
                    MunicipioID = table.Column<int>(nullable: false),
                    Telefono = table.Column<string>(maxLength: 15, nullable: true),
                    TelefonoMovil = table.Column<string>(maxLength: 15, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: false),
                    Activo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteID);
                    table.ForeignKey(
                        name: "FK_Clientes_Municipios_MunicipioID",
                        column: x => x.MunicipioID,
                        principalTable: "Municipios",
                        principalColumn: "MunicipioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Colaboradores",
                columns: table => new
                {
                    ColaboradorID = table.Column<Guid>(nullable: false),
                    CURP = table.Column<string>(maxLength: 18, nullable: false),
                    Nombre = table.Column<string>(maxLength: 75, nullable: false),
                    PrimerApellido = table.Column<string>(maxLength: 75, nullable: false),
                    SegundoApellido = table.Column<string>(maxLength: 75, nullable: true),
                    PuestoID = table.Column<Guid>(nullable: false),
                    GeneroID = table.Column<string>(nullable: true),
                    EstadoNacimientoID = table.Column<short>(nullable: false),
                    FechaNacimiento = table.Column<DateTime>(nullable: false),
                    EstadoCivilID = table.Column<short>(nullable: false),
                    Domicilio = table.Column<string>(maxLength: 100, nullable: false),
                    Colonia = table.Column<string>(maxLength: 100, nullable: false),
                    CodigoPostal = table.Column<int>(nullable: false),
                    MunicipioID = table.Column<int>(nullable: false),
                    Telefono = table.Column<string>(maxLength: 15, nullable: true),
                    TelefonoMovil = table.Column<string>(maxLength: 15, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: false),
                    Activo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colaboradores", x => x.ColaboradorID);
                    table.ForeignKey(
                        name: "FK_Colaboradores_EstadosCiviles_EstadoCivilID",
                        column: x => x.EstadoCivilID,
                        principalTable: "EstadosCiviles",
                        principalColumn: "EstadoCivilID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Colaboradores_Estados_EstadoNacimientoID",
                        column: x => x.EstadoNacimientoID,
                        principalTable: "Estados",
                        principalColumn: "EstadoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Colaboradores_Generos_GeneroID",
                        column: x => x.GeneroID,
                        principalTable: "Generos",
                        principalColumn: "GeneroID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Colaboradores_Municipios_MunicipioID",
                        column: x => x.MunicipioID,
                        principalTable: "Municipios",
                        principalColumn: "MunicipioID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Colaboradores_Puestos_PuestoID",
                        column: x => x.PuestoID,
                        principalTable: "Puestos",
                        principalColumn: "PuestoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    ProveedorID = table.Column<Guid>(nullable: false),
                    RFC = table.Column<string>(maxLength: 13, nullable: false),
                    Nombre = table.Column<string>(maxLength: 75, nullable: false),
                    Contacto = table.Column<string>(maxLength: 100, nullable: false),
                    Domicilio = table.Column<string>(maxLength: 100, nullable: false),
                    Colonia = table.Column<string>(maxLength: 100, nullable: false),
                    CodigoPostal = table.Column<int>(nullable: false),
                    MunicipioID = table.Column<int>(nullable: false),
                    Telefono = table.Column<string>(maxLength: 15, nullable: true),
                    TelefonoMovil = table.Column<string>(maxLength: 15, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: false),
                    Activo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.ProveedorID);
                    table.ForeignKey(
                        name: "FK_Proveedores_Municipios_MunicipioID",
                        column: x => x.MunicipioID,
                        principalTable: "Municipios",
                        principalColumn: "MunicipioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paquetes",
                columns: table => new
                {
                    PaqueteID = table.Column<Guid>(nullable: false),
                    PaqueteProductoID = table.Column<Guid>(nullable: false),
                    PiezaProductoID = table.Column<Guid>(nullable: false),
                    CantidadProductoxPaquete = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paquetes", x => x.PaqueteID);
                    table.ForeignKey(
                        name: "FK_Paquetes_Productos_PaqueteProductoID",
                        column: x => x.PaqueteProductoID,
                        principalTable: "Productos",
                        principalColumn: "ProductoID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Paquetes_Productos_PiezaProductoID",
                        column: x => x.PiezaProductoID,
                        principalTable: "Productos",
                        principalColumn: "ProductoID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ClienteContactos",
                columns: table => new
                {
                    ClienteContactoID = table.Column<Guid>(nullable: false),
                    ClienteID = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 75, nullable: false),
                    PrimerApellido = table.Column<string>(maxLength: 75, nullable: false),
                    SegundoApellido = table.Column<string>(maxLength: 75, nullable: true),
                    TelefonoMovil = table.Column<string>(maxLength: 15, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteContactos", x => x.ClienteContactoID);
                    table.ForeignKey(
                        name: "FK_ClienteContactos_Clientes_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Clientes",
                        principalColumn: "ClienteID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProveedorContactos",
                columns: table => new
                {
                    ProveedorContactoID = table.Column<Guid>(nullable: false),
                    ProveedorID = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 75, nullable: false),
                    PrimerApellido = table.Column<string>(maxLength: 75, nullable: false),
                    SegundoApellido = table.Column<string>(maxLength: 75, nullable: true),
                    TelefonoMovil = table.Column<string>(maxLength: 15, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProveedorContactos", x => x.ProveedorContactoID);
                    table.ForeignKey(
                        name: "FK_ProveedorContactos_Proveedores_ProveedorID",
                        column: x => x.ProveedorID,
                        principalTable: "Proveedores",
                        principalColumn: "ProveedorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Estados",
                columns: new[] { "EstadoID", "EstadoClave", "EstadoDescripcion" },
                values: new object[,]
                {
                    { (short)1, "AS", "AGUASCALIENTES" },
                    { (short)21, "PL", "PUEBLA" },
                    { (short)22, "QT", "QUERETARO DE ARTEAGA" },
                    { (short)23, "QR", "QUINTANA ROO" },
                    { (short)24, "SP", "SAN LUIS POTOSI" },
                    { (short)25, "SL", "SINALOA" },
                    { (short)26, "SR", "SONORA" },
                    { (short)20, "OC", "OAXACA" },
                    { (short)27, "TC", "TABASCO" },
                    { (short)29, "TL", "TLAXCALA" },
                    { (short)30, "VZ", "VERACRUZ" },
                    { (short)31, "YN", "YUCATAN" },
                    { (short)32, "ZS", "ZACATECAS" },
                    { (short)39, "SM", "SERVICIO EXTERIOR MEXICANO" },
                    { (short)99, "NI", "NO IDENTIFICABLE" },
                    { (short)28, "TS", "TAMAULIPAS" },
                    { (short)19, "NL", "NUEVO LEON" },
                    { (short)40, "NE", "NACIDO EN EL EXTRANJERO" },
                    { (short)17, "MS", "MORELOS" },
                    { (short)18, "NT", "NAYARIT" },
                    { (short)3, "BS", "BAJA CALIFORNIA SUR" },
                    { (short)4, "CC", "CAMPECHE" },
                    { (short)5, "CL", "COAHUILA DE ZARAGOZA" },
                    { (short)6, "CM", "COLIMA" },
                    { (short)7, "CS", "CHIAPAS" },
                    { (short)8, "CH", "CHIHUAHUA" },
                    { (short)2, "BC", "BAJA CALIFORNIA" },
                    { (short)10, "DG", "DURANGO" },
                    { (short)11, "GT", "GUANAJUATO" },
                    { (short)12, "GR", "GUERRERO" },
                    { (short)13, "HG", "HIDALGO" },
                    { (short)14, "JC", "JALISCO" },
                    { (short)15, "MC", "ESTADO DE MEXICO" },
                    { (short)9, "DF", "DISTRITO FEDERAL" },
                    { (short)16, "MN", "MICHOACAN DE OCAMPO" }
                });

            migrationBuilder.InsertData(
                table: "EstadosCiviles",
                columns: new[] { "EstadoCivilID", "EstadoCivilDescripcion" },
                values: new object[,]
                {
                    { (short)2, "CASADO" },
                    { (short)1, "SOLTERO" }
                });

            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "GeneroID", "GeneroDescripcion" },
                values: new object[,]
                {
                    { "F", "FEMENINO" },
                    { "M", "MASCULINO" }
                });

            migrationBuilder.InsertData(
                table: "Municipios",
                columns: new[] { "MunicipioID", "Estado", "EstadoID", "MunicipioClave", "MunicipioDescripcion" },
                values: new object[,]
                {
                    { 670, null, (short)15, (short)14, "ATLACOMULCO" },
                    { 668, null, (short)15, (short)12, "ATIZAPAN" },
                    { 667, null, (short)15, (short)11, "ATENCO" },
                    { 669, null, (short)15, (short)13, "ATIZAPAN DE ZARAGOZA" },
                    { 666, null, (short)15, (short)10, "APAXCO" },
                    { 661, null, (short)15, (short)5, "ALMOLOYA DE JUAREZ" },
                    { 664, null, (short)15, (short)8, "AMATEPEC" },
                    { 663, null, (short)15, (short)7, "AMANALCO" },
                    { 662, null, (short)15, (short)6, "ALMOLOYA DEL RIO" },
                    { 671, null, (short)15, (short)15, "ATLAUTLA" },
                    { 660, null, (short)15, (short)4, "ALMOLOYA DE ALQUISIRAS" },
                    { 665, null, (short)15, (short)9, "AMECAMECA" },
                    { 672, null, (short)15, (short)16, "AXAPUSCO" },
                    { 683, null, (short)15, (short)27, "CHAPULTEPEC" },
                    { 674, null, (short)15, (short)18, "CALIMAYA" },
                    { 675, null, (short)15, (short)19, "CAPULHUAC" },
                    { 676, null, (short)15, (short)20, "COACALCO DE BERRIOZABAL" },
                    { 677, null, (short)15, (short)21, "COATEPEC HARINAS" },
                    { 678, null, (short)15, (short)22, "COCOTITLAN" },
                    { 679, null, (short)15, (short)23, "COYOTEPEC" },
                    { 680, null, (short)15, (short)24, "CUAUTITLAN" },
                    { 681, null, (short)15, (short)25, "CHALCO" },
                    { 682, null, (short)15, (short)26, "CHAPA DE MOTA" },
                    { 684, null, (short)15, (short)28, "CHIAUTLA" },
                    { 685, null, (short)15, (short)29, "CHICOLOAPAN" },
                    { 686, null, (short)15, (short)30, "CHICONCUAC" },
                    { 659, null, (short)15, (short)3, "ACULCO" },
                    { 673, null, (short)15, (short)17, "AYAPANGO" },
                    { 658, null, (short)15, (short)2, "ACOLMAN" },
                    { 639, null, (short)14, (short)107, "TUXCUECA" },
                    { 656, null, (short)14, (short)124, "ZAPOTLANEJO" },
                    { 629, null, (short)14, (short)97, "TLAJOMULCO DE ZUÃ?Â?IGA" },
                    { 630, null, (short)14, (short)98, "TLAQUEPAQUE" },
                    { 631, null, (short)14, (short)99, "TOLIMAN" },
                    { 632, null, (short)14, (short)100, "TOMATLAN" },
                    { 633, null, (short)14, (short)101, "TONALA" },
                    { 634, null, (short)14, (short)102, "TONAYA" },
                    { 635, null, (short)14, (short)103, "TONILA" },
                    { 636, null, (short)14, (short)104, "TOTATICHE" },
                    { 637, null, (short)14, (short)105, "TOTOTLAN" },
                    { 638, null, (short)14, (short)106, "TUXCACUESCO" },
                    { 687, null, (short)15, (short)31, "CHIMALHUACAN" },
                    { 640, null, (short)14, (short)108, "TUXPAN" },
                    { 641, null, (short)14, (short)109, "UNION DE SAN ANTONIO" },
                    { 642, null, (short)14, (short)110, "UNION DE TULA" },
                    { 643, null, (short)14, (short)111, "VALLE DE GUADALUPE" },
                    { 644, null, (short)14, (short)112, "VALLE DE JUAREZ" },
                    { 645, null, (short)14, (short)113, "SAN GABRIEL" },
                    { 646, null, (short)14, (short)114, "VILLA CORONA" },
                    { 647, null, (short)14, (short)115, "VILLA GUERRERO" },
                    { 648, null, (short)14, (short)116, "VILLA HIDALGO" },
                    { 649, null, (short)14, (short)117, "CAÃ?Â?ADAS DE OBREGON" },
                    { 650, null, (short)14, (short)118, "YAHUALICA DE GONZALEZ GALLO" },
                    { 651, null, (short)14, (short)119, "ZACOALCO DE TORRES" },
                    { 652, null, (short)14, (short)120, "ZAPOPAN" },
                    { 653, null, (short)14, (short)121, "ZAPOTILTIC" },
                    { 654, null, (short)14, (short)122, "ZAPOTITLAN DE VADILLO" },
                    { 655, null, (short)14, (short)123, "ZAPOTLAN DEL REY" },
                    { 657, null, (short)15, (short)1, "ACAMBAY" },
                    { 688, null, (short)15, (short)32, "DONATO GUERRA" },
                    { 708, null, (short)15, (short)52, "MALINALCO" },
                    { 690, null, (short)15, (short)34, "ECATZINGO" },
                    { 723, null, (short)15, (short)67, "OTZOLOTEPEC" },
                    { 724, null, (short)15, (short)68, "OZUMBA" },
                    { 725, null, (short)15, (short)69, "PAPALOTLA" },
                    { 726, null, (short)15, (short)70, "LA PAZ" },
                    { 727, null, (short)15, (short)71, "POLOTITLAN" },
                    { 728, null, (short)15, (short)72, "RAYON" },
                    { 729, null, (short)15, (short)73, "SAN ANTONIO LA ISLA" },
                    { 730, null, (short)15, (short)74, "SAN FELIPE DEL PROGRESO" },
                    { 731, null, (short)15, (short)75, "SAN MARTIN DE LAS PIRAMIDES" },
                    { 732, null, (short)15, (short)76, "SAN MATEO ATENCO" },
                    { 733, null, (short)15, (short)77, "SAN SIMON DE GUERRERO" },
                    { 734, null, (short)15, (short)78, "SANTO TOMAS" },
                    { 735, null, (short)15, (short)79, "SOYANIQUILPAN DE JUAREZ" },
                    { 736, null, (short)15, (short)80, "SULTEPEC" },
                    { 737, null, (short)15, (short)81, "TECAMAC" },
                    { 738, null, (short)15, (short)82, "TEJUPILCO" },
                    { 739, null, (short)15, (short)83, "TEMAMATLA" },
                    { 740, null, (short)15, (short)84, "TEMASCALAPA" },
                    { 741, null, (short)15, (short)85, "TEMASCALCINGO" },
                    { 742, null, (short)15, (short)86, "TEMASCALTEPEC" },
                    { 743, null, (short)15, (short)87, "TEMOAYA" },
                    { 744, null, (short)15, (short)88, "TENANCINGO" },
                    { 745, null, (short)15, (short)89, "TENANGO DEL AIRE" },
                    { 746, null, (short)15, (short)90, "TENANGO DEL VALLE" },
                    { 747, null, (short)15, (short)91, "TEOLOYUCAN" },
                    { 748, null, (short)15, (short)92, "TEOTIHUACAN" },
                    { 749, null, (short)15, (short)93, "TEPETLAOXTOC" },
                    { 722, null, (short)15, (short)66, "OTZOLOAPAN" },
                    { 721, null, (short)15, (short)65, "OTUMBA" },
                    { 720, null, (short)15, (short)64, "EL ORO" },
                    { 719, null, (short)15, (short)63, "OCUILAN" },
                    { 691, null, (short)15, (short)35, "HUEHUETOCA" },
                    { 692, null, (short)15, (short)36, "HUEYPOXTLA" },
                    { 693, null, (short)15, (short)37, "HUIXQUILUCAN" },
                    { 694, null, (short)15, (short)38, "ISIDRO FABELA" },
                    { 695, null, (short)15, (short)39, "IXTAPALUCA" },
                    { 696, null, (short)15, (short)40, "IXTAPAN DE LA SAL" },
                    { 697, null, (short)15, (short)41, "IXTAPAN DEL ORO" },
                    { 698, null, (short)15, (short)42, "IXTLAHUACA" },
                    { 699, null, (short)15, (short)43, "XALATLACO" },
                    { 700, null, (short)15, (short)44, "JALTENCO" },
                    { 701, null, (short)15, (short)45, "JILOTEPEC" },
                    { 702, null, (short)15, (short)46, "JILOTZINGO" },
                    { 703, null, (short)15, (short)47, "JIQUIPILCO" },
                    { 689, null, (short)15, (short)33, "ECATEPEC DE MORELOS" },
                    { 704, null, (short)15, (short)48, "JOCOTITLAN" },
                    { 706, null, (short)15, (short)50, "JUCHITEPEC" },
                    { 707, null, (short)15, (short)51, "LERMA" },
                    { 628, null, (short)14, (short)96, "TIZAPAN EL ALTO" },
                    { 709, null, (short)15, (short)53, "MELCHOR OCAMPO" },
                    { 710, null, (short)15, (short)54, "METEPEC" },
                    { 711, null, (short)15, (short)55, "MEXICALTZINGO" },
                    { 712, null, (short)15, (short)56, "MORELOS" },
                    { 713, null, (short)15, (short)57, "NAUCALPAN DE JUAREZ" },
                    { 714, null, (short)15, (short)58, "NEZAHUALCOYOTL" },
                    { 715, null, (short)15, (short)59, "NEXTLALPAN" },
                    { 716, null, (short)15, (short)60, "NICOLAS ROMERO" },
                    { 717, null, (short)15, (short)61, "NOPALTEPEC" },
                    { 718, null, (short)15, (short)62, "OCOYOACAC" },
                    { 705, null, (short)15, (short)49, "JOQUICINGO" },
                    { 627, null, (short)14, (short)95, "TEUCHITLAN" },
                    { 607, null, (short)14, (short)75, "SAN MARCOS" },
                    { 625, null, (short)14, (short)93, "TEPATITLAN DE MORELOS" },
                    { 535, null, (short)14, (short)3, "AHUALULCO DE MERCADO" },
                    { 536, null, (short)14, (short)4, "AMACUECA" },
                    { 537, null, (short)14, (short)5, "AMATITAN" },
                    { 538, null, (short)14, (short)6, "AMECA" },
                    { 539, null, (short)14, (short)7, "SAN JUANITO DE ESCOBEDO" },
                    { 540, null, (short)14, (short)8, "ARANDAS" },
                    { 541, null, (short)14, (short)9, "EL ARENAL" },
                    { 542, null, (short)14, (short)10, "ATEMAJAC DE BRIZUELA" },
                    { 543, null, (short)14, (short)11, "ATENGO" },
                    { 544, null, (short)14, (short)12, "ATENGUILLO" },
                    { 545, null, (short)14, (short)13, "ATOTONILCO EL ALTO" },
                    { 546, null, (short)14, (short)14, "ATOYAC" },
                    { 547, null, (short)14, (short)15, "AUTLAN DE NAVARRO" },
                    { 548, null, (short)14, (short)16, "AYOTLAN" },
                    { 549, null, (short)14, (short)17, "AYUTLA" },
                    { 550, null, (short)14, (short)18, "LA BARCA" },
                    { 551, null, (short)14, (short)19, "BOLAÃ?Â?OS" },
                    { 552, null, (short)14, (short)20, "CABO CORRIENTES" },
                    { 553, null, (short)14, (short)21, "CASIMIRO CASTILLO" },
                    { 554, null, (short)14, (short)22, "CIHUATLAN" },
                    { 555, null, (short)14, (short)23, "ZAPOTLAN EL GRANDE" },
                    { 556, null, (short)14, (short)24, "COCULA" },
                    { 557, null, (short)14, (short)25, "COLOTLAN" },
                    { 558, null, (short)14, (short)26, "CONCEPCION DE BUENOS AIRES" },
                    { 559, null, (short)14, (short)27, "CUAUTITLAN DE GARCIA BARRAGAN" },
                    { 560, null, (short)14, (short)28, "CUAUTLA" },
                    { 561, null, (short)14, (short)29, "CUQUIO" },
                    { 534, null, (short)14, (short)2, "ACATLAN DE JUAREZ" },
                    { 533, null, (short)14, (short)1, "ACATIC" },
                    { 532, null, (short)13, (short)84, "ZIMAPAN" },
                    { 531, null, (short)13, (short)83, "ZEMPOALA" },
                    { 503, null, (short)13, (short)55, "SANTIAGO DE ANAYA" },
                    { 504, null, (short)13, (short)56, "SANTIAGO TULANTEPEC DE LUGO GUERRERO" },
                    { 505, null, (short)13, (short)57, "SINGUILUCAN" },
                    { 506, null, (short)13, (short)58, "TASQUILLO" },
                    { 507, null, (short)13, (short)59, "TECOZAUTLA" },
                    { 508, null, (short)13, (short)60, "TENANGO DE DORIA" },
                    { 509, null, (short)13, (short)61, "TEPEAPULCO" },
                    { 510, null, (short)13, (short)62, "TEPEHUACAN DE GUERRERO" },
                    { 511, null, (short)13, (short)63, "TEPEJI DEL RIO DE OCAMPO" },
                    { 512, null, (short)13, (short)64, "TEPETITLAN" },
                    { 513, null, (short)13, (short)65, "TETEPANGO" },
                    { 514, null, (short)13, (short)66, "VILLA DE TEZONTEPEC" },
                    { 515, null, (short)13, (short)67, "TEZONTEPEC DE ALDAMA" },
                    { 562, null, (short)14, (short)30, "CHAPALA" },
                    { 516, null, (short)13, (short)68, "TIANGUISTENGO" },
                    { 518, null, (short)13, (short)70, "TLAHUELILPAN" },
                    { 519, null, (short)13, (short)71, "TLAHUILTEPA" },
                    { 520, null, (short)13, (short)72, "TLANALAPA" },
                    { 521, null, (short)13, (short)73, "TLANCHINOL" },
                    { 522, null, (short)13, (short)74, "TLAXCOAPAN" },
                    { 523, null, (short)13, (short)75, "TOLCAYUCA" },
                    { 524, null, (short)13, (short)76, "TULA DE ALLENDE" },
                    { 525, null, (short)13, (short)77, "TULANCINGO DE BRAVO" },
                    { 526, null, (short)13, (short)78, "XOCHIATIPAN" },
                    { 527, null, (short)13, (short)79, "XOCHICOATLAN" },
                    { 528, null, (short)13, (short)80, "YAHUALICA" },
                    { 529, null, (short)13, (short)81, "ZACUALTIPAN DE ANGELES" },
                    { 530, null, (short)13, (short)82, "ZAPOTLAN DE JUAREZ" },
                    { 517, null, (short)13, (short)69, "TIZAYUCA" },
                    { 626, null, (short)14, (short)94, "TEQUILA" },
                    { 563, null, (short)14, (short)31, "CHIMALTITAN" },
                    { 565, null, (short)14, (short)33, "DEGOLLADO" },
                    { 598, null, (short)14, (short)66, "PONCITLAN" },
                    { 599, null, (short)14, (short)67, "PUERTO VALLARTA" },
                    { 600, null, (short)14, (short)68, "VILLA PURIFICACION" },
                    { 601, null, (short)14, (short)69, "QUITUPAN" },
                    { 602, null, (short)14, (short)70, "EL SALTO" },
                    { 603, null, (short)14, (short)71, "SAN CRISTOBAL DE LA BARRANCA" },
                    { 604, null, (short)14, (short)72, "SAN DIEGO DE ALEJANDRIA" },
                    { 605, null, (short)14, (short)73, "SAN JUAN DE LOS LAGOS" },
                    { 606, null, (short)14, (short)74, "SAN JULIAN" },
                    { 750, null, (short)15, (short)94, "TEPETLIXPA" },
                    { 608, null, (short)14, (short)76, "SAN MARTIN DE BOLAÃ?Â?OS" },
                    { 609, null, (short)14, (short)77, "SAN MARTIN HIDALGO" },
                    { 610, null, (short)14, (short)78, "SAN MIGUEL EL ALTO" },
                    { 611, null, (short)14, (short)79, "GOMEZ FARIAS" },
                    { 612, null, (short)14, (short)80, "SAN SEBASTIAN DEL OESTE" },
                    { 613, null, (short)14, (short)81, "SANTA MARIA DE LOS ANGELES" },
                    { 614, null, (short)14, (short)82, "SAYULA" },
                    { 615, null, (short)14, (short)83, "TALA" },
                    { 616, null, (short)14, (short)84, "TALPA DE ALLENDE" },
                    { 617, null, (short)14, (short)85, "TAMAZULA DE GORDIANO" },
                    { 618, null, (short)14, (short)86, "TAPALPA" },
                    { 619, null, (short)14, (short)87, "TECALITLAN" },
                    { 620, null, (short)14, (short)88, "TECOLOTLAN" },
                    { 621, null, (short)14, (short)89, "TECHALUTA DE MONTENEGRO" },
                    { 622, null, (short)14, (short)90, "TENAMAXTLAN" },
                    { 623, null, (short)14, (short)91, "TEOCALTICHE" },
                    { 624, null, (short)14, (short)92, "TEOCUITATLAN DE CORONA" },
                    { 597, null, (short)14, (short)65, "PIHUAMO" },
                    { 596, null, (short)14, (short)64, "OJUELOS DE JALISCO" },
                    { 595, null, (short)14, (short)63, "OCOTLAN" },
                    { 594, null, (short)14, (short)62, "MIXTLAN" },
                    { 566, null, (short)14, (short)34, "EJUTLA" },
                    { 567, null, (short)14, (short)35, "ENCARNACION DE DIAZ" },
                    { 568, null, (short)14, (short)36, "ETZATLAN" },
                    { 569, null, (short)14, (short)37, "EL GRULLO" },
                    { 570, null, (short)14, (short)38, "GUACHINANGO" },
                    { 571, null, (short)14, (short)39, "GUADALAJARA" },
                    { 572, null, (short)14, (short)40, "HOSTOTIPAQUILLO" },
                    { 573, null, (short)14, (short)41, "HUEJUCAR" },
                    { 574, null, (short)14, (short)42, "HUEJUQUILLA EL ALTO" },
                    { 575, null, (short)14, (short)43, "LA HUERTA" },
                    { 576, null, (short)14, (short)44, "IXTLAHUACAN DE LOS MEMBRILLOS" },
                    { 577, null, (short)14, (short)45, "IXTLAHUACAN DEL RIO" },
                    { 578, null, (short)14, (short)46, "JALOSTOTITLAN" },
                    { 564, null, (short)14, (short)32, "CHIQUILISTLAN" },
                    { 579, null, (short)14, (short)47, "JAMAY" },
                    { 581, null, (short)14, (short)49, "JILOTLAN DE LOS DOLORES" },
                    { 582, null, (short)14, (short)50, "JOCOTEPEC" },
                    { 583, null, (short)14, (short)51, "JUANACATLAN" },
                    { 584, null, (short)14, (short)52, "JUCHITLAN" },
                    { 585, null, (short)14, (short)53, "LAGOS DE MORENO" },
                    { 586, null, (short)14, (short)54, "EL LIMON" },
                    { 587, null, (short)14, (short)55, "MAGDALENA" },
                    { 588, null, (short)14, (short)56, "SANTA MARIA DEL ORO" },
                    { 589, null, (short)14, (short)57, "LA MANZANILLA DE LA PAZ" },
                    { 590, null, (short)14, (short)58, "MASCOTA" },
                    { 591, null, (short)14, (short)59, "MAZAMITLA" },
                    { 592, null, (short)14, (short)60, "MEXTICACAN" },
                    { 593, null, (short)14, (short)61, "MEZQUITIC" },
                    { 580, null, (short)14, (short)48, "JESUS MARIA" },
                    { 751, null, (short)15, (short)95, "TEPOTZOTLAN" },
                    { 770, null, (short)15, (short)114, "VILLA VICTORIA" },
                    { 753, null, (short)15, (short)97, "TEXCALTITLAN" },
                    { 911, null, (short)17, (short)16, "OCUITUCO" },
                    { 912, null, (short)17, (short)17, "PUENTE DE IXTLA" },
                    { 913, null, (short)17, (short)18, "TEMIXCO" },
                    { 914, null, (short)17, (short)19, "TEPALCINGO" },
                    { 915, null, (short)17, (short)20, "TEPOZTLAN" },
                    { 916, null, (short)17, (short)21, "TETECALA" },
                    { 917, null, (short)17, (short)22, "TETELA DEL VOLCAN" },
                    { 918, null, (short)17, (short)23, "TLALNEPANTLA" },
                    { 919, null, (short)17, (short)24, "TLALTIZAPAN" },
                    { 920, null, (short)17, (short)25, "TLAQUILTENANGO" },
                    { 921, null, (short)17, (short)26, "TLAYACAPAN" },
                    { 922, null, (short)17, (short)27, "TOTOLAPAN" },
                    { 923, null, (short)17, (short)28, "XOCHITEPEC" },
                    { 924, null, (short)17, (short)29, "YAUTEPEC" },
                    { 925, null, (short)17, (short)30, "YECAPIXTLA" },
                    { 926, null, (short)17, (short)31, "ZACATEPEC DE HIDALGO" },
                    { 927, null, (short)17, (short)32, "ZACUALPAN DE AMILPAS" },
                    { 928, null, (short)17, (short)33, "TEMOAC" },
                    { 929, null, (short)18, (short)1, "ACAPONETA" },
                    { 930, null, (short)18, (short)2, "AHUACATLAN" },
                    { 931, null, (short)18, (short)3, "AMATLAN DE CAÃ?Â?AS" },
                    { 932, null, (short)18, (short)4, "COMPOSTELA" },
                    { 933, null, (short)18, (short)5, "HUAJICORI" },
                    { 934, null, (short)18, (short)6, "IXTLAN DEL RIO" },
                    { 935, null, (short)18, (short)7, "JALA" },
                    { 936, null, (short)18, (short)8, "XALISCO" },
                    { 937, null, (short)18, (short)9, "DEL NAYAR" },
                    { 910, null, (short)17, (short)15, "MIACATLAN" },
                    { 938, null, (short)18, (short)10, "ROSAMORADA" },
                    { 909, null, (short)17, (short)14, "MAZATEPEC" },
                    { 907, null, (short)17, (short)12, "JOJUTLA" },
                    { 880, null, (short)16, (short)98, "TUXPAN" },
                    { 881, null, (short)16, (short)99, "TUZANTLA" },
                    { 882, null, (short)16, (short)100, "TZINTZUNTZAN" },
                    { 883, null, (short)16, (short)101, "TZITZIO" },
                    { 884, null, (short)16, (short)102, "URUAPAN" },
                    { 885, null, (short)16, (short)103, "VENUSTIANO CARRANZA" },
                    { 886, null, (short)16, (short)104, "VILLAMAR" },
                    { 887, null, (short)16, (short)105, "VISTA HERMOSA" },
                    { 888, null, (short)16, (short)106, "YURECUARO" },
                    { 889, null, (short)16, (short)107, "ZACAPU" },
                    { 890, null, (short)16, (short)108, "ZAMORA" },
                    { 891, null, (short)16, (short)109, "ZINAPARO" },
                    { 892, null, (short)16, (short)110, "ZINAPECUARO" },
                    { 893, null, (short)16, (short)111, "ZIRACUARETIRO" },
                    { 894, null, (short)16, (short)112, "ZITACUARO" },
                    { 895, null, (short)16, (short)113, "JOSE SIXTO VERDUZCO" },
                    { 896, null, (short)17, (short)1, "AMACUZAC" },
                    { 897, null, (short)17, (short)2, "ATLATLAHUCAN" },
                    { 898, null, (short)17, (short)3, "AXOCHIAPAN" },
                    { 899, null, (short)17, (short)4, "AYALA" },
                    { 900, null, (short)17, (short)5, "COATLAN DEL RIO" },
                    { 901, null, (short)17, (short)6, "CUAUTLA" },
                    { 902, null, (short)17, (short)7, "CUERNAVACA" },
                    { 903, null, (short)17, (short)8, "EMILIANO ZAPATA" },
                    { 904, null, (short)17, (short)9, "HUITZILAC" },
                    { 905, null, (short)17, (short)10, "JANTETELCO" },
                    { 906, null, (short)17, (short)11, "JIUTEPEC" },
                    { 908, null, (short)17, (short)13, "JONACATEPEC" },
                    { 939, null, (short)18, (short)11, "RUIZ" },
                    { 940, null, (short)18, (short)12, "SAN BLAS" },
                    { 941, null, (short)18, (short)13, "SAN PEDRO LAGUNILLAS" },
                    { 974, null, (short)19, (short)26, "GUADALUPE" },
                    { 975, null, (short)19, (short)27, "LOS HERRERAS" },
                    { 976, null, (short)19, (short)28, "HIGUERAS" },
                    { 977, null, (short)19, (short)29, "HUALAHUISES" },
                    { 978, null, (short)19, (short)30, "ITURBIDE" },
                    { 979, null, (short)19, (short)31, "JUAREZ" },
                    { 980, null, (short)19, (short)32, "LAMPAZOS DE NARANJO" },
                    { 981, null, (short)19, (short)33, "LINARES" },
                    { 982, null, (short)19, (short)34, "MARIN" },
                    { 983, null, (short)19, (short)35, "MELCHOR OCAMPO" },
                    { 984, null, (short)19, (short)36, "MIER Y NORIEGA" },
                    { 985, null, (short)19, (short)37, "MINA" },
                    { 986, null, (short)19, (short)38, "MONTEMORELOS" },
                    { 987, null, (short)19, (short)39, "MONTERREY" },
                    { 988, null, (short)19, (short)40, "PARAS" },
                    { 989, null, (short)19, (short)41, "PESQUERIA" },
                    { 990, null, (short)19, (short)42, "LOS RAMONES" },
                    { 991, null, (short)19, (short)43, "RAYONES" },
                    { 992, null, (short)19, (short)44, "SABINAS HIDALGO" },
                    { 993, null, (short)19, (short)45, "SALINAS VICTORIA" },
                    { 994, null, (short)19, (short)46, "SAN NICOLAS DE LOS GARZA" },
                    { 995, null, (short)19, (short)47, "HIDALGO" },
                    { 996, null, (short)19, (short)48, "SANTA CATARINA" },
                    { 997, null, (short)19, (short)49, "SANTIAGO" },
                    { 998, null, (short)19, (short)50, "VALLECILLO" },
                    { 999, null, (short)19, (short)51, "VILLALDAMA" },
                    { 1000, null, (short)20, (short)1, "ABEJONES" },
                    { 973, null, (short)19, (short)25, "GRAL. ZUAZUA" },
                    { 972, null, (short)19, (short)24, "GRAL. ZARAGOZA" },
                    { 971, null, (short)19, (short)23, "GRAL. TREVIÃ?Â?O" },
                    { 970, null, (short)19, (short)22, "GRAL. TERAN" },
                    { 942, null, (short)18, (short)14, "SANTA MARIA DEL ORO" },
                    { 943, null, (short)18, (short)15, "SANTIAGO IXCUINTLA" },
                    { 944, null, (short)18, (short)16, "TECUALA" },
                    { 945, null, (short)18, (short)17, "TEPIC" },
                    { 946, null, (short)18, (short)18, "TUXPAN" },
                    { 947, null, (short)18, (short)19, "LA YESCA" },
                    { 948, null, (short)18, (short)20, "BAHIA DE BANDERAS" },
                    { 949, null, (short)19, (short)1, "ABASOLO" },
                    { 950, null, (short)19, (short)2, "AGUALEGUAS" },
                    { 951, null, (short)19, (short)3, "LOS ALDAMAS" },
                    { 952, null, (short)19, (short)4, "ALLENDE" },
                    { 953, null, (short)19, (short)5, "ANAHUAC" },
                    { 954, null, (short)19, (short)6, "APODACA" },
                    { 879, null, (short)16, (short)97, "TURICATO" },
                    { 955, null, (short)19, (short)7, "ARAMBERRI" },
                    { 957, null, (short)19, (short)9, "CADEREYTA JIMENEZ" },
                    { 958, null, (short)19, (short)10, "CARMEN" },
                    { 959, null, (short)19, (short)11, "CERRALVO" },
                    { 960, null, (short)19, (short)12, "CIENEGA DE FLORES" },
                    { 961, null, (short)19, (short)13, "CHINA" },
                    { 962, null, (short)19, (short)14, "DR. ARROYO" },
                    { 963, null, (short)19, (short)15, "DR. COSS" },
                    { 964, null, (short)19, (short)16, "DR. GONZALEZ" },
                    { 965, null, (short)19, (short)17, "GALEANA" },
                    { 966, null, (short)19, (short)18, "GARCIA" },
                    { 967, null, (short)19, (short)19, "SAN PEDRO GARZA GARCIA" },
                    { 968, null, (short)19, (short)20, "GRAL. BRAVO" },
                    { 969, null, (short)19, (short)21, "GRAL. ESCOBEDO" },
                    { 956, null, (short)19, (short)8, "BUSTAMANTE" },
                    { 878, null, (short)16, (short)96, "TUMBISCATIO" },
                    { 877, null, (short)16, (short)95, "TOCUMBO" },
                    { 876, null, (short)16, (short)94, "TLAZAZALCA" },
                    { 786, null, (short)16, (short)4, "ANGAMACUTIRO" },
                    { 787, null, (short)16, (short)5, "ANGANGUEO" },
                    { 788, null, (short)16, (short)6, "APATZINGAN" },
                    { 789, null, (short)16, (short)7, "APORO" },
                    { 790, null, (short)16, (short)8, "AQUILA" },
                    { 791, null, (short)16, (short)9, "ARIO" },
                    { 792, null, (short)16, (short)10, "ARTEAGA" },
                    { 793, null, (short)16, (short)11, "BRISEÃ?Â?AS" },
                    { 794, null, (short)16, (short)12, "BUENAVISTA" },
                    { 795, null, (short)16, (short)13, "CARACUARO" },
                    { 796, null, (short)16, (short)14, "COAHUAYANA" },
                    { 797, null, (short)16, (short)15, "COALCOMAN DE VAZQUEZ PALLARES" },
                    { 798, null, (short)16, (short)16, "COENEO" },
                    { 799, null, (short)16, (short)17, "CONTEPEC" },
                    { 800, null, (short)16, (short)18, "COPANDARO" },
                    { 801, null, (short)16, (short)19, "COTIJA" },
                    { 802, null, (short)16, (short)20, "CUITZEO" },
                    { 803, null, (short)16, (short)21, "CHARAPAN" },
                    { 804, null, (short)16, (short)22, "CHARO" },
                    { 805, null, (short)16, (short)23, "CHAVINDA" },
                    { 806, null, (short)16, (short)24, "CHERAN" }
                });

            migrationBuilder.InsertData(
                table: "Municipios",
                columns: new[] { "MunicipioID", "Estado", "EstadoID", "MunicipioClave", "MunicipioDescripcion" },
                values: new object[,]
                {
                    { 807, null, (short)16, (short)25, "CHILCHOTA" },
                    { 808, null, (short)16, (short)26, "CHINICUILA" },
                    { 809, null, (short)16, (short)27, "CHUCANDIRO" },
                    { 810, null, (short)16, (short)28, "CHURINTZIO" },
                    { 811, null, (short)16, (short)29, "CHURUMUCO" },
                    { 812, null, (short)16, (short)30, "ECUANDUREO" },
                    { 785, null, (short)16, (short)3, "ALVARO OBREGON" },
                    { 784, null, (short)16, (short)2, "AGUILILLA" },
                    { 783, null, (short)16, (short)1, "ACUITZIO" },
                    { 782, null, (short)15, (short)999, "ILEGIBLE" },
                    { 754, null, (short)15, (short)98, "TEXCALYACAC" },
                    { 755, null, (short)15, (short)99, "TEXCOCO" },
                    { 756, null, (short)15, (short)100, "TEZOYUCA" },
                    { 757, null, (short)15, (short)101, "TIANGUISTENCO" },
                    { 758, null, (short)15, (short)102, "TIMILPAN" },
                    { 759, null, (short)15, (short)103, "TLALMANALCO" },
                    { 760, null, (short)15, (short)104, "TLALNEPANTLA DE BAZ" },
                    { 761, null, (short)15, (short)105, "TLATLAYA" },
                    { 762, null, (short)15, (short)106, "TOLUCA" },
                    { 763, null, (short)15, (short)107, "TONATICO" },
                    { 764, null, (short)15, (short)108, "TULTEPEC" },
                    { 765, null, (short)15, (short)109, "TULTITLAN" },
                    { 766, null, (short)15, (short)110, "VALLE DE BRAVO" },
                    { 813, null, (short)16, (short)31, "EPITACIO HUERTA" },
                    { 767, null, (short)15, (short)111, "VILLA DE ALLENDE" },
                    { 769, null, (short)15, (short)113, "VILLA GUERRERO" },
                    { 502, null, (short)13, (short)54, "SAN SALVADOR" },
                    { 771, null, (short)15, (short)115, "XONACATLAN" },
                    { 772, null, (short)15, (short)116, "ZACAZONAPAN" },
                    { 773, null, (short)15, (short)117, "ZACUALPAN" },
                    { 774, null, (short)15, (short)118, "ZINACANTEPEC" },
                    { 775, null, (short)15, (short)119, "ZUMPAHUACAN" },
                    { 776, null, (short)15, (short)120, "ZUMPANGO" },
                    { 777, null, (short)15, (short)121, "CUAUTITLAN IZCALLI" },
                    { 778, null, (short)15, (short)122, "VALLE DE CHALCO SOLIDARIDAD" },
                    { 779, null, (short)15, (short)123, "LUVIANOS" },
                    { 780, null, (short)15, (short)124, "SAN JOSE DEL RINCON" },
                    { 781, null, (short)15, (short)125, "TONANITLA" },
                    { 768, null, (short)15, (short)112, "VILLA DEL CARBON" },
                    { 752, null, (short)15, (short)96, "TEQUIXQUIAC" },
                    { 814, null, (short)16, (short)32, "ERONGARICUARO" },
                    { 816, null, (short)16, (short)34, "HIDALGO" },
                    { 849, null, (short)16, (short)67, "PENJAMILLO" },
                    { 850, null, (short)16, (short)68, "PERIBAN" },
                    { 851, null, (short)16, (short)69, "LA PIEDAD" },
                    { 852, null, (short)16, (short)70, "PUREPERO" },
                    { 853, null, (short)16, (short)71, "PURUANDIRO" },
                    { 854, null, (short)16, (short)72, "QUERENDARO" },
                    { 855, null, (short)16, (short)73, "QUIROGA" },
                    { 856, null, (short)16, (short)74, "COJUMATLAN DE REGULES" },
                    { 857, null, (short)16, (short)75, "LOS REYES" },
                    { 858, null, (short)16, (short)76, "SAHUAYO" },
                    { 859, null, (short)16, (short)77, "SAN LUCAS" },
                    { 860, null, (short)16, (short)78, "SANTA ANA MAYA" },
                    { 861, null, (short)16, (short)79, "SALVADOR ESCALANTE" },
                    { 862, null, (short)16, (short)80, "SENGUIO" },
                    { 863, null, (short)16, (short)81, "SUSUPUATO" },
                    { 864, null, (short)16, (short)82, "TACAMBARO" },
                    { 865, null, (short)16, (short)83, "TANCITARO" },
                    { 866, null, (short)16, (short)84, "TANGAMANDAPIO" },
                    { 867, null, (short)16, (short)85, "TANGANCICUARO" },
                    { 868, null, (short)16, (short)86, "TANHUATO" },
                    { 869, null, (short)16, (short)87, "TARETAN" },
                    { 870, null, (short)16, (short)88, "TARIMBARO" },
                    { 871, null, (short)16, (short)89, "TEPALCATEPEC" },
                    { 872, null, (short)16, (short)90, "TINGAMBATO" },
                    { 873, null, (short)16, (short)91, "TING" },
                    { 874, null, (short)16, (short)92, "TIQUICHEO DE NICOLAS ROMERO" },
                    { 875, null, (short)16, (short)93, "TLALPUJAHUA" },
                    { 848, null, (short)16, (short)66, "PATZCUARO" },
                    { 847, null, (short)16, (short)65, "PARACHO" },
                    { 846, null, (short)16, (short)64, "PARACUARO" },
                    { 845, null, (short)16, (short)63, "PANINDICUARO" },
                    { 817, null, (short)16, (short)35, "LA HUACANA" },
                    { 818, null, (short)16, (short)36, "HUANDACAREO" },
                    { 819, null, (short)16, (short)37, "HUANIQUEO" },
                    { 820, null, (short)16, (short)38, "HUETAMO" },
                    { 821, null, (short)16, (short)39, "HUIRAMBA" },
                    { 822, null, (short)16, (short)40, "INDAPARAPEO" },
                    { 823, null, (short)16, (short)41, "IRIMBO" },
                    { 824, null, (short)16, (short)42, "IXTLAN" },
                    { 825, null, (short)16, (short)43, "JACONA" },
                    { 826, null, (short)16, (short)44, "JIMENEZ" },
                    { 827, null, (short)16, (short)45, "JIQUILPAN" },
                    { 828, null, (short)16, (short)46, "JUAREZ" },
                    { 829, null, (short)16, (short)47, "JUNGAPEO" },
                    { 815, null, (short)16, (short)33, "GABRIEL ZAMORA" },
                    { 830, null, (short)16, (short)48, "LAGUNILLAS" },
                    { 832, null, (short)16, (short)50, "MARAVATIO" },
                    { 833, null, (short)16, (short)51, "MARCOS CASTELLANOS" },
                    { 834, null, (short)16, (short)52, "LAZARO CARDENAS" },
                    { 835, null, (short)16, (short)53, "MORELIA" },
                    { 836, null, (short)16, (short)54, "MORELOS" },
                    { 837, null, (short)16, (short)55, "MUGICA" },
                    { 838, null, (short)16, (short)56, "NAHUATZEN" },
                    { 839, null, (short)16, (short)57, "NOCUPETARO" },
                    { 840, null, (short)16, (short)58, "NUEVO PARANGARICUTIRO" },
                    { 841, null, (short)16, (short)59, "NUEVO URECHO" },
                    { 842, null, (short)16, (short)60, "NUMARAN" },
                    { 843, null, (short)16, (short)61, "OCAMPO" },
                    { 844, null, (short)16, (short)62, "PAJACUARAN" },
                    { 831, null, (short)16, (short)49, "MADERO" },
                    { 501, null, (short)13, (short)53, "SAN BARTOLO TUTOTEPEC" },
                    { 483, null, (short)13, (short)35, "METEPEC" },
                    { 499, null, (short)13, (short)51, "MINERAL DE LA REFORMA" },
                    { 158, null, (short)7, (short)78, "SAN CRISTOBAL DE LAS CASAS" },
                    { 159, null, (short)7, (short)79, "SAN FERNANDO" },
                    { 160, null, (short)7, (short)80, "SILTEPEC" },
                    { 161, null, (short)7, (short)81, "SIMOJOVEL" },
                    { 162, null, (short)7, (short)82, "SITALA" },
                    { 163, null, (short)7, (short)83, "SOCOLTENANGO" },
                    { 164, null, (short)7, (short)84, "SOLOSUCHIAPA" },
                    { 165, null, (short)7, (short)85, "SOYALO" },
                    { 166, null, (short)7, (short)86, "SUCHIAPA" },
                    { 167, null, (short)7, (short)87, "SUCHIATE" },
                    { 168, null, (short)7, (short)88, "SUNUAPA" },
                    { 169, null, (short)7, (short)89, "TAPACHULA" },
                    { 170, null, (short)7, (short)90, "TAPALAPA" },
                    { 171, null, (short)7, (short)91, "TAPILULA" },
                    { 172, null, (short)7, (short)92, "TECPATAN" },
                    { 173, null, (short)7, (short)93, "TENEJAPA" },
                    { 174, null, (short)7, (short)94, "TEOPISCA" },
                    { 175, null, (short)7, (short)96, "TILA" },
                    { 176, null, (short)7, (short)97, "TONALA" },
                    { 177, null, (short)7, (short)98, "TOTOLAPA" },
                    { 178, null, (short)7, (short)99, "LA TRINITARIA" },
                    { 179, null, (short)7, (short)100, "TUMBALA" },
                    { 180, null, (short)7, (short)101, "TUXTLA GUTIÉRREZ" },
                    { 181, null, (short)7, (short)102, "TUXTLA CHICO" },
                    { 182, null, (short)7, (short)103, "TUZANTAN" },
                    { 183, null, (short)7, (short)104, "TZIMOL" },
                    { 184, null, (short)7, (short)105, "UNION JUAREZ" },
                    { 157, null, (short)7, (short)77, "SALTO DE AGUA" },
                    { 185, null, (short)7, (short)106, "VENUSTIANO CARRANZA" },
                    { 156, null, (short)7, (short)76, "SABANILLA" },
                    { 154, null, (short)7, (short)74, "REFORMA" },
                    { 127, null, (short)7, (short)47, "JITOTOL" },
                    { 128, null, (short)7, (short)48, "JUAREZ" },
                    { 129, null, (short)7, (short)49, "LARRAINZAR" },
                    { 130, null, (short)7, (short)50, "LA LIBERTAD" },
                    { 131, null, (short)7, (short)51, "MAPASTEPEC" },
                    { 132, null, (short)7, (short)52, "LAS MARGARITAS" },
                    { 133, null, (short)7, (short)53, "MAZAPA DE MADERO" },
                    { 134, null, (short)7, (short)54, "MAZATAN" },
                    { 135, null, (short)7, (short)55, "METAPA" },
                    { 136, null, (short)7, (short)56, "MITONTIC" },
                    { 137, null, (short)7, (short)57, "MOTOZINTLA" },
                    { 138, null, (short)7, (short)58, "NICOLAS RUIZ" },
                    { 139, null, (short)7, (short)59, "OCOSINGO" },
                    { 140, null, (short)7, (short)60, "OCOTEPEC" },
                    { 141, null, (short)7, (short)61, "OCOZOCOAUTLA DE ESPINOSA" },
                    { 142, null, (short)7, (short)62, "OSTUACAN" },
                    { 143, null, (short)7, (short)63, "OSUMACINTA" },
                    { 144, null, (short)7, (short)64, "OXCHUC" },
                    { 145, null, (short)7, (short)65, "PALENQUE" },
                    { 146, null, (short)7, (short)66, "PANTELHO" },
                    { 147, null, (short)7, (short)67, "PANTEPEC" },
                    { 148, null, (short)7, (short)68, "PICHUCALCO" },
                    { 149, null, (short)7, (short)69, "PIJIJIAPAN" },
                    { 150, null, (short)7, (short)70, "EL PORVENIR" },
                    { 151, null, (short)7, (short)71, "VILLA COMALTITLAN" },
                    { 152, null, (short)7, (short)72, "PUEBLO NUEVO SOLISTAHUACAN" },
                    { 153, null, (short)7, (short)73, "RAYON" },
                    { 155, null, (short)7, (short)75, "LAS ROSAS" },
                    { 186, null, (short)7, (short)107, "VILLA CORZO" },
                    { 187, null, (short)7, (short)108, "VILLAFLORES" },
                    { 188, null, (short)7, (short)109, "YAJALON" },
                    { 221, null, (short)8, (short)23, "GALEANA" },
                    { 222, null, (short)8, (short)24, "SANTA ISABEL" },
                    { 223, null, (short)8, (short)25, "GOMEZ FARIAS" },
                    { 224, null, (short)8, (short)26, "GRAN MORELOS" },
                    { 225, null, (short)8, (short)27, "GUACHOCHI" },
                    { 226, null, (short)8, (short)28, "GUADALUPE" },
                    { 227, null, (short)8, (short)29, "GUADALUPE Y CALVO" },
                    { 228, null, (short)8, (short)30, "GUAZAPARES" },
                    { 229, null, (short)8, (short)31, "GUERRERO" },
                    { 230, null, (short)8, (short)32, "HIDALGO DEL PARRAL" },
                    { 231, null, (short)8, (short)33, "HUEJOTITAN" },
                    { 232, null, (short)8, (short)34, "IGNACIO ZARAGOZA" },
                    { 233, null, (short)8, (short)35, "JANOS" },
                    { 234, null, (short)8, (short)36, "JIMENEZ" },
                    { 235, null, (short)8, (short)37, "JUAREZ" },
                    { 236, null, (short)8, (short)38, "JULIMES" },
                    { 237, null, (short)8, (short)39, "LOPEZ" },
                    { 238, null, (short)8, (short)40, "MADERA" },
                    { 239, null, (short)8, (short)41, "MAGUARICHI" },
                    { 240, null, (short)8, (short)42, "MANUEL BENAVIDES" },
                    { 241, null, (short)8, (short)43, "MATACHI" },
                    { 242, null, (short)8, (short)44, "MATAMOROS" },
                    { 243, null, (short)8, (short)45, "MEOQUI" },
                    { 244, null, (short)8, (short)46, "MORELOS" },
                    { 245, null, (short)8, (short)47, "MORIS" },
                    { 246, null, (short)8, (short)48, "NAMIQUIPA" },
                    { 247, null, (short)8, (short)49, "NONOAVA" },
                    { 220, null, (short)8, (short)22, "DR. BELISARIO DOMINGUEZ" },
                    { 219, null, (short)8, (short)21, "DELICIAS" },
                    { 218, null, (short)8, (short)20, "CHINIPAS" },
                    { 217, null, (short)8, (short)19, "CHIHUAHUA" },
                    { 189, null, (short)7, (short)110, "SAN LUCAS" },
                    { 190, null, (short)7, (short)111, "ZINACANTAN" },
                    { 191, null, (short)7, (short)112, "SAN JUAN CANCUC" },
                    { 192, null, (short)7, (short)113, "ALDAMA" },
                    { 193, null, (short)7, (short)114, "BENEMERITO DE LAS AMERICAS" },
                    { 194, null, (short)7, (short)115, "MARAVILLA TENEJAPA" },
                    { 195, null, (short)7, (short)116, "MARQUES DE COMILLAS" },
                    { 196, null, (short)7, (short)117, "MONTECRISTO DE GUERRERO" },
                    { 197, null, (short)7, (short)118, "SAN ANDRES DURAZNAL" },
                    { 198, null, (short)7, (short)119, "SANTIAGO EL PINAR" },
                    { 199, null, (short)8, (short)1, "AHUMADA" },
                    { 200, null, (short)8, (short)2, "ALDAMA" },
                    { 201, null, (short)8, (short)3, "ALLENDE" },
                    { 126, null, (short)7, (short)46, "JIQUIPILAS" },
                    { 202, null, (short)8, (short)4, "AQUILES SERDAN" },
                    { 204, null, (short)8, (short)6, "BACHINIVA" },
                    { 205, null, (short)8, (short)7, "BALLEZA" },
                    { 206, null, (short)8, (short)8, "BATOPILAS" },
                    { 207, null, (short)8, (short)9, "BOCOYNA" },
                    { 208, null, (short)8, (short)10, "BUENAVENTURA" },
                    { 209, null, (short)8, (short)11, "CAMARGO" },
                    { 210, null, (short)8, (short)12, "CARICHI" },
                    { 211, null, (short)8, (short)13, "CASAS GRANDES" },
                    { 212, null, (short)8, (short)14, "CORONADO" },
                    { 213, null, (short)8, (short)15, "COYAME DEL SOTOL" },
                    { 214, null, (short)8, (short)16, "LA CRUZ" },
                    { 215, null, (short)8, (short)17, "CUAUHTEMOC" },
                    { 216, null, (short)8, (short)18, "CUSIHUIRIACHI" },
                    { 203, null, (short)8, (short)5, "ASCENSION" },
                    { 125, null, (short)7, (short)45, "IXTAPANGAJOYA" },
                    { 124, null, (short)7, (short)44, "IXTAPA" },
                    { 123, null, (short)7, (short)43, "IXTACOMITAN" },
                    { 33, null, (short)5, (short)1, "ABASOLO" },
                    { 34, null, (short)5, (short)2, "ACUÃ?Â?A" },
                    { 35, null, (short)5, (short)3, "ALLENDE" },
                    { 36, null, (short)5, (short)4, "ARTEAGA" },
                    { 37, null, (short)5, (short)5, "CANDELA" },
                    { 38, null, (short)5, (short)6, "CASTAÃ?Â?OS" },
                    { 39, null, (short)5, (short)7, "CUATRO CIENEGAS" },
                    { 40, null, (short)5, (short)8, "ESCOBEDO" },
                    { 41, null, (short)5, (short)9, "FRANCISCO I. MADERO" },
                    { 42, null, (short)5, (short)10, "FRONTERA" },
                    { 43, null, (short)5, (short)11, "GENERAL CEPEDA" },
                    { 44, null, (short)5, (short)12, "GUERRERO" },
                    { 45, null, (short)5, (short)13, "HIDALGO" },
                    { 46, null, (short)5, (short)14, "JIMENEZ" },
                    { 47, null, (short)5, (short)15, "JUAREZ" },
                    { 48, null, (short)5, (short)16, "LAMADRID" },
                    { 49, null, (short)5, (short)17, "MATAMOROS" },
                    { 50, null, (short)5, (short)18, "MONCLOVA" },
                    { 51, null, (short)5, (short)19, "MORELOS" },
                    { 52, null, (short)5, (short)20, "MUZQUIZ" },
                    { 53, null, (short)5, (short)21, "NADADORES" },
                    { 54, null, (short)5, (short)22, "NAVA" },
                    { 55, null, (short)5, (short)23, "OCAMPO" },
                    { 56, null, (short)5, (short)24, "PARRAS" },
                    { 57, null, (short)5, (short)25, "PIEDRAS NEGRAS" },
                    { 58, null, (short)5, (short)26, "PROGRESO" },
                    { 59, null, (short)5, (short)27, "RAMOS ARIZPE" },
                    { 32, null, (short)4, (short)11, "CANDELARIA" },
                    { 31, null, (short)4, (short)10, "CALAKMUL" },
                    { 30, null, (short)4, (short)9, "ESCARCEGA" },
                    { 29, null, (short)4, (short)8, "TENABO" },
                    { 1, null, (short)1, (short)1, "AGUASCALIENTES" },
                    { 2, null, (short)1, (short)2, "ASIENTOS" },
                    { 3, null, (short)1, (short)3, "CALVILLO" },
                    { 4, null, (short)1, (short)4, "COSIO" },
                    { 5, null, (short)1, (short)5, "JESUS MARIA" },
                    { 6, null, (short)1, (short)6, "PABELLON DE ARTEAGA" },
                    { 7, null, (short)1, (short)7, "RINCON DE ROMOS" },
                    { 8, null, (short)1, (short)8, "SAN JOSE DE GRACIA" },
                    { 9, null, (short)1, (short)9, "TEPEZALA" },
                    { 10, null, (short)1, (short)10, "EL LLANO" },
                    { 11, null, (short)1, (short)11, "SAN FRANCISCO DE LOS ROMO" },
                    { 12, null, (short)2, (short)1, "ENSENADA" },
                    { 13, null, (short)2, (short)2, "MEXICALI" },
                    { 60, null, (short)5, (short)28, "SABINAS" },
                    { 14, null, (short)2, (short)3, "TECATE" },
                    { 16, null, (short)2, (short)5, "PLAYAS DE ROSARITO" },
                    { 17, null, (short)3, (short)1, "COMONDU" },
                    { 18, null, (short)3, (short)2, "MULEGE" },
                    { 19, null, (short)3, (short)3, "LA PAZ" },
                    { 20, null, (short)3, (short)8, "LOS CABOS" },
                    { 21, null, (short)3, (short)9, "LORETO" },
                    { 22, null, (short)4, (short)1, "CALKINI" },
                    { 23, null, (short)4, (short)2, "CAMPECHE" },
                    { 24, null, (short)4, (short)3, "CARMEN" },
                    { 25, null, (short)4, (short)4, "CHAMPOTON" },
                    { 26, null, (short)4, (short)5, "HECELCHAKAN" },
                    { 27, null, (short)4, (short)6, "HOPELCHEN" },
                    { 28, null, (short)4, (short)7, "PALIZADA" },
                    { 15, null, (short)2, (short)4, "TIJUANA" },
                    { 500, null, (short)13, (short)52, "SAN AGUSTIN TLAXIACA" },
                    { 61, null, (short)5, (short)29, "SACRAMENTO" },
                    { 63, null, (short)5, (short)31, "SAN BUENAVENTURA" },
                    { 96, null, (short)7, (short)16, "CATAZAJA" },
                    { 97, null, (short)7, (short)17, "CINTALAPA" },
                    { 98, null, (short)7, (short)18, "COAPILLA" },
                    { 99, null, (short)7, (short)19, "COMITAN DE DOMINGUEZ" },
                    { 100, null, (short)7, (short)20, "LA CONCORDIA" },
                    { 101, null, (short)7, (short)21, "COPAINALA" },
                    { 102, null, (short)7, (short)22, "CHALCHIHUITAN" },
                    { 103, null, (short)7, (short)23, "CHAMULA" },
                    { 104, null, (short)7, (short)24, "CHANAL" },
                    { 105, null, (short)7, (short)25, "CHAPULTENANGO" },
                    { 106, null, (short)7, (short)26, "CHENALHO" },
                    { 107, null, (short)7, (short)27, "CHIAPA DE CORZO" },
                    { 108, null, (short)7, (short)28, "CHIAPILLA" },
                    { 109, null, (short)7, (short)29, "CHICOASEN" },
                    { 110, null, (short)7, (short)30, "CHICOMUSELO" },
                    { 111, null, (short)7, (short)31, "CHILON" },
                    { 112, null, (short)7, (short)32, "ESCUINTLA" },
                    { 113, null, (short)7, (short)33, "FRANCISCO LEON" },
                    { 114, null, (short)7, (short)34, "FRONTERA COMALAPA" },
                    { 115, null, (short)7, (short)35, "FRONTERA HIDALGO" },
                    { 116, null, (short)7, (short)36, "LA GRANDEZA" },
                    { 117, null, (short)7, (short)37, "HUEHUETAN" },
                    { 118, null, (short)7, (short)38, "HUIXTAN" },
                    { 119, null, (short)7, (short)39, "HUITIUPAN" },
                    { 120, null, (short)7, (short)40, "HUIXTLA" },
                    { 121, null, (short)7, (short)41, "LA INDEPENDENCIA" },
                    { 122, null, (short)7, (short)42, "IXHUATAN" },
                    { 95, null, (short)7, (short)15, "CACAHOATAN" },
                    { 94, null, (short)7, (short)14, "EL BOSQUE" },
                    { 93, null, (short)7, (short)13, "BOCHIL" },
                    { 92, null, (short)7, (short)12, "BERRIOZABAL" },
                    { 64, null, (short)5, (short)32, "SAN JUAN DE SABINAS" },
                    { 65, null, (short)5, (short)33, "SAN PEDRO" },
                    { 66, null, (short)5, (short)34, "SIERRA MOJADA" },
                    { 67, null, (short)5, (short)35, "TORREON" },
                    { 68, null, (short)5, (short)36, "VIESCA" },
                    { 69, null, (short)5, (short)37, "VILLA UNION" },
                    { 70, null, (short)5, (short)38, "ZARAGOZA" },
                    { 71, null, (short)6, (short)1, "ARMERIA" },
                    { 72, null, (short)6, (short)2, "COLIMA" },
                    { 73, null, (short)6, (short)3, "COMALA" },
                    { 74, null, (short)6, (short)4, "COQUIMATLAN" },
                    { 75, null, (short)6, (short)5, "CUAUHTEMOC" },
                    { 76, null, (short)6, (short)6, "IXTLAHUACAN" },
                    { 62, null, (short)5, (short)30, "SALTILLO" },
                    { 77, null, (short)6, (short)7, "MANZANILLO" },
                    { 79, null, (short)6, (short)9, "TECOMAN" },
                    { 80, null, (short)6, (short)10, "VILLA DE ALVAREZ" },
                    { 81, null, (short)7, (short)1, "ACACOYAGUA" },
                    { 82, null, (short)7, (short)2, "ACALA" },
                    { 83, null, (short)7, (short)3, "ACAPETAHUA" },
                    { 84, null, (short)7, (short)4, "ALTAMIRANO" },
                    { 85, null, (short)7, (short)5, "AMATAN" },
                    { 86, null, (short)7, (short)6, "AMATENANGO DE LA FRONTERA" },
                    { 87, null, (short)7, (short)7, "AMATENANGO DEL VALLE" },
                    { 88, null, (short)7, (short)8, "ANGEL ALBINO CORZO" },
                    { 89, null, (short)7, (short)9, "ARRIAGA" },
                    { 90, null, (short)7, (short)10, "BEJUCAL DE OCAMPO" },
                    { 91, null, (short)7, (short)11, "BELLA VISTA" },
                    { 78, null, (short)6, (short)8, "MINATITLAN" },
                    { 249, null, (short)8, (short)51, "OCAMPO" },
                    { 248, null, (short)8, (short)50, "NUEVO CASAS GRANDES" },
                    { 251, null, (short)8, (short)53, "PRAXEDIS G. GUERRERO" },
                    { 408, null, (short)12, (short)41, "MALINALTEPEC" },
                    { 409, null, (short)12, (short)42, "MARTIR DE CUILAPAN" },
                    { 410, null, (short)12, (short)43, "METLATONOC" },
                    { 411, null, (short)12, (short)44, "MOCHITLAN" },
                    { 412, null, (short)12, (short)45, "OLINALA" },
                    { 413, null, (short)12, (short)46, "OMETEPEC" },
                    { 414, null, (short)12, (short)47, "PEDRO ASCENCIO ALQUISIRAS" },
                    { 415, null, (short)12, (short)48, "PETATLAN" },
                    { 416, null, (short)12, (short)49, "PILCAYA" },
                    { 417, null, (short)12, (short)50, "PUNGARABATO" },
                    { 418, null, (short)12, (short)51, "QUECHULTENANGO" },
                    { 419, null, (short)12, (short)52, "SAN LUIS ACATLAN" },
                    { 420, null, (short)12, (short)53, "SAN MARCOS" },
                    { 421, null, (short)12, (short)54, "SAN MIGUEL TOTOLAPAN" },
                    { 422, null, (short)12, (short)55, "TAXCO DE ALARCON" },
                    { 423, null, (short)12, (short)56, "TECOANAPA" },
                    { 424, null, (short)12, (short)57, "TECPAN DE GALEANA" },
                    { 425, null, (short)12, (short)58, "TELOLOAPAN" },
                    { 426, null, (short)12, (short)59, "TEPECOACUILCO DE TRUJANO" },
                    { 427, null, (short)12, (short)60, "TETIPAC" },
                    { 428, null, (short)12, (short)61, "TIXTLA DE GUERRERO" },
                    { 429, null, (short)12, (short)62, "TLACOACHISTLAHUACA" },
                    { 430, null, (short)12, (short)63, "TLACOAPA" },
                    { 431, null, (short)12, (short)64, "TLALCHAPA" },
                    { 432, null, (short)12, (short)65, "TLALIXTAQUILLA DE MALDONADO" },
                    { 433, null, (short)12, (short)66, "TLAPA DE COMONFORT" },
                    { 434, null, (short)12, (short)67, "TLAPEHUALA" },
                    { 407, null, (short)12, (short)40, "LEONARDO BRAVO" },
                    { 435, null, (short)12, (short)68, "LA UNION DE ISIDORO MONTES DE OCA" },
                    { 406, null, (short)12, (short)39, "JUAN R. ESCUDERO" },
                    { 404, null, (short)12, (short)37, "IXCATEOPAN DE CUAUHTEMOC" },
                    { 250, null, (short)8, (short)52, "OJINAGA" },
                    { 378, null, (short)12, (short)11, "ATOYAC DE ALVAREZ" },
                    { 379, null, (short)12, (short)12, "AYUTLA DE LOS LIBRES" },
                    { 380, null, (short)12, (short)13, "AZOYU" },
                    { 381, null, (short)12, (short)14, "BENITO JUAREZ" },
                    { 382, null, (short)12, (short)15, "BUENAVISTA DE CUELLAR" },
                    { 383, null, (short)12, (short)16, "COAHUAYUTLA DE JOSE MARIA IZAZAGA" },
                    { 384, null, (short)12, (short)17, "COCULA" },
                    { 385, null, (short)12, (short)18, "COPALA" },
                    { 386, null, (short)12, (short)19, "COPALILLO" },
                    { 387, null, (short)12, (short)20, "COPANATOYAC" },
                    { 388, null, (short)12, (short)21, "COYUCA DE BENITEZ" },
                    { 389, null, (short)12, (short)22, "COYUCA DE CATALAN" },
                    { 390, null, (short)12, (short)23, "CUAJINICUILAPA" },
                    { 391, null, (short)12, (short)24, "CUALAC" },
                    { 392, null, (short)12, (short)25, "CUAUTEPEC" },
                    { 393, null, (short)12, (short)26, "CUETZALA DEL PROGRESO" },
                    { 394, null, (short)12, (short)27, "CUTZAMALA DE PINZON" },
                    { 395, null, (short)12, (short)28, "CHILAPA DE ALVAREZ" },
                    { 396, null, (short)12, (short)29, "CHILPANCINGO DE LOS BRAVO" },
                    { 397, null, (short)12, (short)30, "FLORENCIO VILLARREAL" },
                    { 398, null, (short)12, (short)31, "GENERAL CANUTO A. NERI" },
                    { 399, null, (short)12, (short)32, "GENERAL HELIODORO CASTILLO" },
                    { 400, null, (short)12, (short)33, "HUAMUXTITLAN" },
                    { 401, null, (short)12, (short)34, "HUITZUCO DE LOS FIGUEROA" },
                    { 402, null, (short)12, (short)35, "IGUALA DE LA INDEPENDENCIA" },
                    { 403, null, (short)12, (short)36, "IGUALAPA" },
                    { 405, null, (short)12, (short)38, "JOSE AZUETA" },
                    { 436, null, (short)12, (short)69, "XALPATLAHUAC" },
                    { 437, null, (short)12, (short)70, "XOCHIHUEHUETLAN" },
                    { 438, null, (short)12, (short)71, "XOCHISTLAHUACA" },
                    { 471, null, (short)13, (short)23, "FRANCISCO I. MADERO" }
                });

            migrationBuilder.InsertData(
                table: "Municipios",
                columns: new[] { "MunicipioID", "Estado", "EstadoID", "MunicipioClave", "MunicipioDescripcion" },
                values: new object[,]
                {
                    { 472, null, (short)13, (short)24, "HUASCA DE OCAMPO" },
                    { 473, null, (short)13, (short)25, "HUAUTLA" },
                    { 474, null, (short)13, (short)26, "HUAZALINGO" },
                    { 475, null, (short)13, (short)27, "HUEHUETLA" },
                    { 476, null, (short)13, (short)28, "HUEJUTLA DE REYES" },
                    { 477, null, (short)13, (short)29, "HUICHAPAN" },
                    { 478, null, (short)13, (short)30, "IXMIQUILPAN" },
                    { 479, null, (short)13, (short)31, "JACALA DE LEDEZMA" },
                    { 480, null, (short)13, (short)32, "JALTOCAN" },
                    { 481, null, (short)13, (short)33, "JUAREZ HIDALGO" },
                    { 482, null, (short)13, (short)34, "LOLOTLA" },
                    { 484, null, (short)13, (short)36, "SAN AGUSTIN METZQUITITLAN" },
                    { 485, null, (short)13, (short)37, "METZTITLAN" },
                    { 486, null, (short)13, (short)38, "MINERAL DEL CHICO" },
                    { 487, null, (short)13, (short)39, "MINERAL DEL MONTE" },
                    { 488, null, (short)13, (short)40, "LA MISION" },
                    { 489, null, (short)13, (short)41, "MIXQUIAHUALA DE JUAREZ" },
                    { 490, null, (short)13, (short)42, "MOLANGO DE ESCAMILLA" },
                    { 491, null, (short)13, (short)43, "NICOLAS FLORES" },
                    { 492, null, (short)13, (short)44, "NOPALA DE VILLAGRAN" },
                    { 493, null, (short)13, (short)45, "OMITLAN DE JUAREZ" },
                    { 494, null, (short)13, (short)46, "SAN FELIPE ORIZATLAN" },
                    { 495, null, (short)13, (short)47, "PACULA" },
                    { 496, null, (short)13, (short)48, "PACHUCA DE SOTO" },
                    { 497, null, (short)13, (short)49, "PISAFLORES" },
                    { 498, null, (short)13, (short)50, "PROGRESO DE OBREGON" },
                    { 470, null, (short)13, (short)22, "EPAZOYUCAN" },
                    { 469, null, (short)13, (short)21, "EMILIANO ZAPATA" },
                    { 468, null, (short)13, (short)20, "ELOXOCHITLAN" },
                    { 467, null, (short)13, (short)19, "CHILCUAUTLA" },
                    { 439, null, (short)12, (short)72, "ZAPOTITLAN TABLAS" },
                    { 440, null, (short)12, (short)73, "ZIRANDARO" },
                    { 441, null, (short)12, (short)74, "ZITLALA" },
                    { 442, null, (short)12, (short)75, "EDUARDO NERI" },
                    { 443, null, (short)12, (short)76, "ACATEPEC" },
                    { 444, null, (short)12, (short)77, "MARQUELIA" },
                    { 445, null, (short)12, (short)78, "COCHOAPA EL GRANDE" },
                    { 446, null, (short)12, (short)79, "JOSE JOAQUIN DE HERRERA" },
                    { 447, null, (short)12, (short)80, "JUCHITAN" },
                    { 448, null, (short)12, (short)81, "ILIATENCO" },
                    { 449, null, (short)13, (short)1, "ACATLAN" },
                    { 450, null, (short)13, (short)2, "ACAXOCHITLAN" },
                    { 451, null, (short)13, (short)3, "ACTOPAN" },
                    { 376, null, (short)12, (short)9, "ATLAMAJALCINGO DEL MONTE" },
                    { 452, null, (short)13, (short)4, "AGUA BLANCA DE ITURBIDE" },
                    { 454, null, (short)13, (short)6, "ALFAJAYUCAN" },
                    { 455, null, (short)13, (short)7, "ALMOLOYA" },
                    { 456, null, (short)13, (short)8, "APAN" },
                    { 457, null, (short)13, (short)9, "EL ARENAL" },
                    { 458, null, (short)13, (short)10, "ATITALAQUIA" },
                    { 459, null, (short)13, (short)11, "ATLAPEXCO" },
                    { 460, null, (short)13, (short)12, "ATOTONILCO EL GRANDE" },
                    { 461, null, (short)13, (short)13, "ATOTONILCO DE TULA" },
                    { 462, null, (short)13, (short)14, "CALNALI" },
                    { 463, null, (short)13, (short)15, "CARDONAL" },
                    { 464, null, (short)13, (short)16, "CUAUTEPEC DE HINOJOSA" },
                    { 465, null, (short)13, (short)17, "CHAPANTONGO" },
                    { 466, null, (short)13, (short)18, "CHAPULHUACAN" },
                    { 453, null, (short)13, (short)5, "AJACUBA" },
                    { 375, null, (short)12, (short)8, "ATENANGO DEL RIO" },
                    { 377, null, (short)12, (short)10, "ATLIXTAC" },
                    { 373, null, (short)12, (short)6, "APAXTLA" },
                    { 283, null, (short)10, (short)1, "CANATLAN" },
                    { 284, null, (short)10, (short)2, "CANELAS" },
                    { 285, null, (short)10, (short)3, "CONETO DE COMONFORT" },
                    { 286, null, (short)10, (short)4, "CUENCAME" },
                    { 287, null, (short)10, (short)5, "DURANGO" },
                    { 288, null, (short)10, (short)6, "GENERAL SIMON BOLIVAR" },
                    { 289, null, (short)10, (short)7, "GOMEZ PALACIO" },
                    { 290, null, (short)10, (short)8, "GUADALUPE VICTORIA" },
                    { 291, null, (short)10, (short)9, "GUANACEVI" },
                    { 292, null, (short)10, (short)10, "HIDALGO" },
                    { 293, null, (short)10, (short)11, "INDE" },
                    { 294, null, (short)10, (short)12, "LERDO" },
                    { 295, null, (short)10, (short)13, "MAPIMI" },
                    { 296, null, (short)10, (short)14, "MEZQUITAL" },
                    { 297, null, (short)10, (short)15, "NAZAS" },
                    { 298, null, (short)10, (short)16, "NOMBRE DE DIOS" },
                    { 299, null, (short)10, (short)17, "OCAMPO" },
                    { 300, null, (short)10, (short)18, "EL ORO" },
                    { 301, null, (short)10, (short)19, "OTAEZ" },
                    { 302, null, (short)10, (short)20, "PANUCO DE CORONADO" },
                    { 303, null, (short)10, (short)21, "PEÃ?Â?ON BLANCO" },
                    { 304, null, (short)10, (short)22, "POANAS" },
                    { 305, null, (short)10, (short)23, "PUEBLO NUEVO" },
                    { 306, null, (short)10, (short)24, "RODEO" },
                    { 307, null, (short)10, (short)25, "SAN BERNARDO" },
                    { 308, null, (short)10, (short)26, "SAN DIMAS" },
                    { 309, null, (short)10, (short)27, "SAN JUAN DE GUADALUPE" },
                    { 282, null, (short)9, (short)999, "ILEGIBLE" },
                    { 374, null, (short)12, (short)7, "ARCELIA" },
                    { 281, null, (short)9, (short)17, "VENUSTIANO CARRANZA" },
                    { 279, null, (short)9, (short)15, "CUAUHTEMOC" },
                    { 252, null, (short)8, (short)54, "RIVA PALACIO" },
                    { 253, null, (short)8, (short)55, "ROSALES" },
                    { 254, null, (short)8, (short)56, "ROSARIO" },
                    { 255, null, (short)8, (short)57, "SAN FRANCISCO DE BORJA" },
                    { 256, null, (short)8, (short)58, "SAN FRANCISCO DE CONCHOS" },
                    { 257, null, (short)8, (short)59, "SAN FRANCISCO DEL ORO" },
                    { 258, null, (short)8, (short)60, "SANTA BARBARA" },
                    { 259, null, (short)8, (short)61, "SATEVO" },
                    { 260, null, (short)8, (short)62, "SAUCILLO" },
                    { 261, null, (short)8, (short)63, "TEMOSACHI" },
                    { 262, null, (short)8, (short)64, "EL TULE" },
                    { 263, null, (short)8, (short)65, "URIQUE" },
                    { 264, null, (short)8, (short)66, "URUACHI" },
                    { 265, null, (short)8, (short)67, "VALLE DE ZARAGOZA" },
                    { 266, null, (short)9, (short)2, "AZCAPOTZALCO" },
                    { 267, null, (short)9, (short)3, "COYOACAN" },
                    { 268, null, (short)9, (short)4, "CUAJIMALPA DE MORELOS" },
                    { 269, null, (short)9, (short)5, "GUSTAVO A. MADERO" },
                    { 270, null, (short)9, (short)6, "IZTACALCO" },
                    { 271, null, (short)9, (short)7, "IZTAPALAPA" },
                    { 272, null, (short)9, (short)8, "LA MAGDALENA CONTRERAS" },
                    { 273, null, (short)9, (short)9, "MILPA ALTA" },
                    { 274, null, (short)9, (short)10, "ALVARO OBREGON" },
                    { 275, null, (short)9, (short)11, "TLAHUAC" },
                    { 276, null, (short)9, (short)12, "TLALPAN" },
                    { 277, null, (short)9, (short)13, "XOCHIMILCO" },
                    { 278, null, (short)9, (short)14, "BENITO JUAREZ" },
                    { 280, null, (short)9, (short)16, "MIGUEL HIDALGO" },
                    { 311, null, (short)10, (short)29, "SAN LUIS DEL CORDERO" },
                    { 310, null, (short)10, (short)28, "SAN JUAN DEL RIO" },
                    { 313, null, (short)10, (short)31, "SANTA CLARA" },
                    { 346, null, (short)11, (short)25, "PURISIMA DEL RINCON" },
                    { 347, null, (short)11, (short)26, "ROMITA" },
                    { 348, null, (short)11, (short)27, "SALAMANCA" },
                    { 349, null, (short)11, (short)28, "SALVATIERRA" },
                    { 350, null, (short)11, (short)29, "SAN DIEGO DE LA UNION" },
                    { 351, null, (short)11, (short)30, "SAN FELIPE" },
                    { 352, null, (short)11, (short)31, "SAN FRANCISCO DEL RINCON" },
                    { 353, null, (short)11, (short)32, "SAN JOSE ITURBIDE" },
                    { 354, null, (short)11, (short)33, "SAN LUIS DE LA PAZ" },
                    { 355, null, (short)11, (short)34, "SANTA CATARINA" },
                    { 356, null, (short)11, (short)35, "SANTA CRUZ DE JUVENTINO ROSAS" },
                    { 357, null, (short)11, (short)36, "SANTIAGO MARAVATIO" },
                    { 358, null, (short)11, (short)37, "SILAO" },
                    { 359, null, (short)11, (short)38, "TARANDACUAO" },
                    { 360, null, (short)11, (short)39, "TARIMORO" },
                    { 361, null, (short)11, (short)40, "TIERRA BLANCA" },
                    { 362, null, (short)11, (short)41, "URIANGATO" },
                    { 363, null, (short)11, (short)42, "VALLE DE SANTIAGO" },
                    { 364, null, (short)11, (short)43, "VICTORIA" },
                    { 365, null, (short)11, (short)44, "VILLAGRAN" },
                    { 366, null, (short)11, (short)45, "XICHU" },
                    { 367, null, (short)11, (short)46, "YURIRIA" },
                    { 368, null, (short)12, (short)1, "ACAPULCO DE JUAREZ" },
                    { 369, null, (short)12, (short)2, "AHUACUOTZINGO" },
                    { 370, null, (short)12, (short)3, "AJUCHITLAN DEL PROGRESO" },
                    { 371, null, (short)12, (short)4, "ALCOZAUCA DE GUERRERO" },
                    { 372, null, (short)12, (short)5, "ALPOYECA" },
                    { 312, null, (short)10, (short)30, "SAN PEDRO DEL GALLO" },
                    { 344, null, (short)11, (short)23, "PENJAMO" },
                    { 345, null, (short)11, (short)24, "PUEBLO NUEVO" },
                    { 342, null, (short)11, (short)21, "MOROLEON" },
                    { 314, null, (short)10, (short)32, "SANTIAGO PAPASQUIARO" },
                    { 315, null, (short)10, (short)33, "SUCHIL" },
                    { 316, null, (short)10, (short)34, "TAMAZULA" },
                    { 317, null, (short)10, (short)35, "TEPEHUANES" },
                    { 318, null, (short)10, (short)36, "TLAHUALILO" },
                    { 319, null, (short)10, (short)37, "TOPIA" },
                    { 320, null, (short)10, (short)38, "VICENTE GUERRERO" },
                    { 321, null, (short)10, (short)39, "NUEVO IDEAL" },
                    { 322, null, (short)11, (short)1, "ABASOLO" },
                    { 323, null, (short)11, (short)2, "ACAMBARO" },
                    { 324, null, (short)11, (short)3, "SAN MIGUEL DE ALLENDE" },
                    { 325, null, (short)11, (short)4, "APASEO EL ALTO" },
                    { 343, null, (short)11, (short)22, "OCAMPO" },
                    { 327, null, (short)11, (short)6, "ATARJEA" },
                    { 326, null, (short)11, (short)5, "APASEO EL GRANDE" },
                    { 337, null, (short)11, (short)16, "HUANIMARO" },
                    { 329, null, (short)11, (short)8, "MANUEL DOBLADO" },
                    { 330, null, (short)11, (short)9, "COMONFORT" },
                    { 331, null, (short)11, (short)10, "CORONEO" },
                    { 332, null, (short)11, (short)11, "CORTAZAR" },
                    { 333, null, (short)11, (short)12, "CUERAMARO" },
                    { 334, null, (short)11, (short)13, "DOCTOR MORA" },
                    { 335, null, (short)11, (short)14, "DOLORES HIDALGO CUNA DE LA INDEPENDENCIA NACIONAL" },
                    { 336, null, (short)11, (short)15, "GUANAJUATO" },
                    { 328, null, (short)11, (short)7, "CELAYA" },
                    { 338, null, (short)11, (short)17, "IRAPUATO" },
                    { 339, null, (short)11, (short)18, "JARAL DEL PROGRESO" },
                    { 340, null, (short)11, (short)19, "JERECUARO" },
                    { 341, null, (short)11, (short)20, "LEON" }
                });

            migrationBuilder.InsertData(
                table: "Puestos",
                columns: new[] { "PuestoID", "PuestoDescripcion", "PuestoNombre" },
                values: new object[] { new Guid("d6d5973b-fa59-4b0f-837a-35f83350a63e"), "DESCRIPCIÓN DEL PUESTO", "PUESTO" });

            migrationBuilder.InsertData(
                table: "Unidades",
                columns: new[] { "UnidadID", "Paquete", "Pieza", "UnidadDescripcion", "UnidadNombre" },
                values: new object[,]
                {
                    { new Guid("6c9c7801-d654-11e9-8b00-8cdcd47d68a1"), true, true, "IDENTIFICADOR DE PAQUETES QUE SE PUEDEN OPERAR CON CANTIDADES ENTERAS", "CAJA" },
                    { new Guid("826671fc-d654-11e9-8b00-8cdcd47d68a1"), false, false, "IDENTIFICADOR DE PRODUCTOS QUE SE PUEDEN OPERAR CON CANTIDADES DECIMALES", "KILOGRAMOS" },
                    { new Guid("401b9552-d654-11e9-8b00-8cdcd47d68a1"), false, true, "IDENTIFICADOR DE PRODUCTOS QUE SE PUEDEN OPERAR CON CANTIDADES ENTERAS", "PIEZA" },
                    { new Guid("95b850ec-d654-11e9-8b00-8cdcd47d68a1"), true, false, "IDENTIFICADOR DE PAQUETES QUE SE PUEDEN OPERAR CON CANTIDADES DECIMALES", "CAJAKG" }
                });

            migrationBuilder.InsertData(
                table: "Colaboradores",
                columns: new[] { "ColaboradorID", "Activo", "CURP", "CodigoPostal", "Colonia", "Domicilio", "Email", "EstadoCivilID", "EstadoNacimientoID", "FechaNacimiento", "FechaRegistro", "GeneroID", "MunicipioID", "Nombre", "PrimerApellido", "PuestoID", "SegundoApellido", "Telefono", "TelefonoMovil" },
                values: new object[] { new Guid("1071ae75-e2cd-4c7d-b120-4b01aff19814"), true, "CURP781227HCSRNS00", 29000, "COLONIA", "DOMICILIO", "administrador@lambusiness.com", (short)2, (short)7, new DateTime(1978, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 9, 26, 2, 21, 29, 141, DateTimeKind.Local).AddTicks(3169), "M", 180, "NOMBRE", "PRIMERAPELLIDO", new Guid("d6d5973b-fa59-4b0f-837a-35f83350a63e"), "SEGUNDOAPELLIDO", "1234567890", "0123456789" });

            migrationBuilder.CreateIndex(
                name: "IX_ClienteContactos_ClienteID",
                table: "ClienteContactos",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_MunicipioID",
                table: "Clientes",
                column: "MunicipioID");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_RFC",
                table: "Clientes",
                column: "RFC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_CURP",
                table: "Colaboradores",
                column: "CURP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_EstadoCivilID",
                table: "Colaboradores",
                column: "EstadoCivilID");

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_EstadoNacimientoID",
                table: "Colaboradores",
                column: "EstadoNacimientoID");

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_GeneroID",
                table: "Colaboradores",
                column: "GeneroID");

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_MunicipioID",
                table: "Colaboradores",
                column: "MunicipioID");

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_PuestoID",
                table: "Colaboradores",
                column: "PuestoID");

            migrationBuilder.CreateIndex(
                name: "IX_Estados_EstadoClave",
                table: "Estados",
                column: "EstadoClave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_Estado",
                table: "Municipios",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Paquetes_PaqueteProductoID",
                table: "Paquetes",
                column: "PaqueteProductoID");

            migrationBuilder.CreateIndex(
                name: "IX_Paquetes_PiezaProductoID",
                table: "Paquetes",
                column: "PiezaProductoID");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Codigo",
                table: "Productos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_TasaID",
                table: "Productos",
                column: "TasaID");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_UnidadID",
                table: "Productos",
                column: "UnidadID");

            migrationBuilder.CreateIndex(
                name: "IX_ProveedorContactos_ProveedorID",
                table: "ProveedorContactos",
                column: "ProveedorID");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_MunicipioID",
                table: "Proveedores",
                column: "MunicipioID");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_RFC",
                table: "Proveedores",
                column: "RFC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Puestos_PuestoNombre",
                table: "Puestos",
                column: "PuestoNombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TasasImpuestos_Tasa",
                table: "TasasImpuestos",
                column: "Tasa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unidades_UnidadNombre",
                table: "Unidades",
                column: "UnidadNombre",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClienteContactos");

            migrationBuilder.DropTable(
                name: "Colaboradores");

            migrationBuilder.DropTable(
                name: "Paquetes");

            migrationBuilder.DropTable(
                name: "ProveedorContactos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "EstadosCiviles");

            migrationBuilder.DropTable(
                name: "Generos");

            migrationBuilder.DropTable(
                name: "Puestos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "TasasImpuestos");

            migrationBuilder.DropTable(
                name: "Unidades");

            migrationBuilder.DropTable(
                name: "Municipios");

            migrationBuilder.DropTable(
                name: "Estados");
        }
    }
}

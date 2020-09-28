namespace LAMBusiness.Web.Data.Entities
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Catalogo;
    public class EstadoConfiguracion: IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            builder.HasData(
                new Estado { EstadoID = 1, EstadoClave = "AS", EstadoDescripcion = "AGUASCALIENTES" },
                new Estado { EstadoID = 2, EstadoClave = "BC", EstadoDescripcion = "BAJA CALIFORNIA" },
                new Estado { EstadoID = 3, EstadoClave = "BS", EstadoDescripcion = "BAJA CALIFORNIA SUR" },
                new Estado { EstadoID = 4, EstadoClave = "CC", EstadoDescripcion = "CAMPECHE" },
                new Estado { EstadoID = 5, EstadoClave = "CL", EstadoDescripcion = "COAHUILA DE ZARAGOZA" },
                new Estado { EstadoID = 6, EstadoClave = "CM", EstadoDescripcion = "COLIMA" },
                new Estado { EstadoID = 7, EstadoClave = "CS", EstadoDescripcion = "CHIAPAS" },
                new Estado { EstadoID = 8, EstadoClave = "CH", EstadoDescripcion = "CHIHUAHUA" },
                new Estado { EstadoID = 9, EstadoClave = "DF", EstadoDescripcion = "DISTRITO FEDERAL" },
                new Estado { EstadoID = 10, EstadoClave = "DG", EstadoDescripcion = "DURANGO" },
                new Estado { EstadoID = 11, EstadoClave = "GT", EstadoDescripcion = "GUANAJUATO" },
                new Estado { EstadoID = 12, EstadoClave = "GR", EstadoDescripcion = "GUERRERO" },
                new Estado { EstadoID = 13, EstadoClave = "HG", EstadoDescripcion = "HIDALGO" },
                new Estado { EstadoID = 14, EstadoClave = "JC", EstadoDescripcion = "JALISCO" },
                new Estado { EstadoID = 15, EstadoClave = "MC", EstadoDescripcion = "ESTADO DE MEXICO" },
                new Estado { EstadoID = 16, EstadoClave = "MN", EstadoDescripcion = "MICHOACAN DE OCAMPO" },
                new Estado { EstadoID = 17, EstadoClave = "MS", EstadoDescripcion = "MORELOS" },
                new Estado { EstadoID = 18, EstadoClave = "NT", EstadoDescripcion = "NAYARIT" },
                new Estado { EstadoID = 19, EstadoClave = "NL", EstadoDescripcion = "NUEVO LEON" },
                new Estado { EstadoID = 20, EstadoClave = "OC", EstadoDescripcion = "OAXACA" },
                new Estado { EstadoID = 21, EstadoClave = "PL", EstadoDescripcion = "PUEBLA" },
                new Estado { EstadoID = 22, EstadoClave = "QT", EstadoDescripcion = "QUERETARO DE ARTEAGA" },
                new Estado { EstadoID = 23, EstadoClave = "QR", EstadoDescripcion = "QUINTANA ROO" },
                new Estado { EstadoID = 24, EstadoClave = "SP", EstadoDescripcion = "SAN LUIS POTOSI" },
                new Estado { EstadoID = 25, EstadoClave = "SL", EstadoDescripcion = "SINALOA" },
                new Estado { EstadoID = 26, EstadoClave = "SR", EstadoDescripcion = "SONORA" },
                new Estado { EstadoID = 27, EstadoClave = "TC", EstadoDescripcion = "TABASCO" },
                new Estado { EstadoID = 28, EstadoClave = "TS", EstadoDescripcion = "TAMAULIPAS" },
                new Estado { EstadoID = 29, EstadoClave = "TL", EstadoDescripcion = "TLAXCALA" },
                new Estado { EstadoID = 30, EstadoClave = "VZ", EstadoDescripcion = "VERACRUZ" },
                new Estado { EstadoID = 31, EstadoClave = "YN", EstadoDescripcion = "YUCATAN" },
                new Estado { EstadoID = 32, EstadoClave = "ZS", EstadoDescripcion = "ZACATECAS" },
                new Estado { EstadoID = 39, EstadoClave = "SM", EstadoDescripcion = "SERVICIO EXTERIOR MEXICANO" },
                new Estado { EstadoID = 40, EstadoClave = "NE", EstadoDescripcion = "NACIDO EN EL EXTRANJERO" },
                new Estado { EstadoID = 99, EstadoClave = "NI", EstadoDescripcion = "NO IDENTIFICABLE" }
            );
        }
    }
}

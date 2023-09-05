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
                new Estado { EstadoID = 1, Clave = "AS", Nombre = "AGUASCALIENTES" },
                new Estado { EstadoID = 2, Clave = "BC", Nombre = "BAJA CALIFORNIA" },
                new Estado { EstadoID = 3, Clave = "BS", Nombre = "BAJA CALIFORNIA SUR" },
                new Estado { EstadoID = 4, Clave = "CC", Nombre = "CAMPECHE" },
                new Estado { EstadoID = 5, Clave = "CL", Nombre = "COAHUILA DE ZARAGOZA" },
                new Estado { EstadoID = 6, Clave = "CM", Nombre = "COLIMA" },
                new Estado { EstadoID = 7, Clave = "CS", Nombre = "CHIAPAS" },
                new Estado { EstadoID = 8, Clave = "CH", Nombre = "CHIHUAHUA" },
                new Estado { EstadoID = 9, Clave = "DF", Nombre = "DISTRITO FEDERAL" },
                new Estado { EstadoID = 10, Clave = "DG", Nombre = "DURANGO" },
                new Estado { EstadoID = 11, Clave = "GT", Nombre = "GUANAJUATO" },
                new Estado { EstadoID = 12, Clave = "GR", Nombre = "GUERRERO" },
                new Estado { EstadoID = 13, Clave = "HG", Nombre = "HIDALGO" },
                new Estado { EstadoID = 14, Clave = "JC", Nombre = "JALISCO" },
                new Estado { EstadoID = 15, Clave = "MC", Nombre = "ESTADO DE MEXICO" },
                new Estado { EstadoID = 16, Clave = "MN", Nombre = "MICHOACAN DE OCAMPO" },
                new Estado { EstadoID = 17, Clave = "MS", Nombre = "MORELOS" },
                new Estado { EstadoID = 18, Clave = "NT", Nombre = "NAYARIT" },
                new Estado { EstadoID = 19, Clave = "NL", Nombre = "NUEVO LEON" },
                new Estado { EstadoID = 20, Clave = "OC", Nombre = "OAXACA" },
                new Estado { EstadoID = 21, Clave = "PL", Nombre = "PUEBLA" },
                new Estado { EstadoID = 22, Clave = "QT", Nombre = "QUERETARO DE ARTEAGA" },
                new Estado { EstadoID = 23, Clave = "QR", Nombre = "QUINTANA ROO" },
                new Estado { EstadoID = 24, Clave = "SP", Nombre = "SAN LUIS POTOSI" },
                new Estado { EstadoID = 25, Clave = "SL", Nombre = "SINALOA" },
                new Estado { EstadoID = 26, Clave = "SR", Nombre = "SONORA" },
                new Estado { EstadoID = 27, Clave = "TC", Nombre = "TABASCO" },
                new Estado { EstadoID = 28, Clave = "TS", Nombre = "TAMAULIPAS" },
                new Estado { EstadoID = 29, Clave = "TL", Nombre = "TLAXCALA" },
                new Estado { EstadoID = 30, Clave = "VZ", Nombre = "VERACRUZ" },
                new Estado { EstadoID = 31, Clave = "YN", Nombre = "YUCATAN" },
                new Estado { EstadoID = 32, Clave = "ZS", Nombre = "ZACATECAS" },
                new Estado { EstadoID = 39, Clave = "SM", Nombre = "SERVICIO EXTERIOR MEXICANO" },
                new Estado { EstadoID = 40, Clave = "NE", Nombre = "NACIDO EN EL EXTRANJERO" },
                new Estado { EstadoID = 99, Clave = "NI", Nombre = "NO IDENTIFICABLE" }
            );
        }
    }
}

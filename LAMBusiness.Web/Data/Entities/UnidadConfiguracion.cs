namespace LAMBusiness.Web.Data.Entities
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Catalogo;
    using System;

    public class UnidadConfiguracion : IEntityTypeConfiguration<Unidad>
    {
        public void Configure(EntityTypeBuilder<Unidad> builder)
        {
            builder.HasData(
                new Unidad { UnidadID = Guid.Parse("401b9552-d654-11e9-8b00-8cdcd47d68a1"), Nombre = "PIEZA", Descripcion = "IDENTIFICADOR DE PRODUCTOS QUE SE PUEDEN OPERAR CON CANTIDADES ENTERAS", Pieza = true, Paquete = false },
                new Unidad { UnidadID = Guid.Parse("6c9c7801-d654-11e9-8b00-8cdcd47d68a1"), Nombre = "CAJA", Descripcion = "IDENTIFICADOR DE PAQUETES QUE SE PUEDEN OPERAR CON CANTIDADES ENTERAS", Pieza = true, Paquete = true },
                new Unidad { UnidadID = Guid.Parse("826671fc-d654-11e9-8b00-8cdcd47d68a1"), Nombre = "KILOGRAMOS", Descripcion = "IDENTIFICADOR DE PRODUCTOS QUE SE PUEDEN OPERAR CON CANTIDADES DECIMALES", Pieza = false, Paquete = false },
                new Unidad { UnidadID = Guid.Parse("95b850ec-d654-11e9-8b00-8cdcd47d68a1"), Nombre = "CAJAKG", Descripcion = "IDENTIFICADOR DE PAQUETES QUE SE PUEDEN OPERAR CON CANTIDADES DECIMALES", Pieza = false, Paquete = true }
            );
        }
    }
}

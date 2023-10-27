namespace LAMBusiness.Contextos.Entities
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Catalogo;

    public class AlmacenConfiguracion : IEntityTypeConfiguration<Almacen>
    {
        public void Configure(EntityTypeBuilder<Almacen> builder)
        {
            builder.HasData(
                new Almacen {
                    AlmacenID = Guid.Parse("8706EF28-2EBA-463A-BAB4-62227965F03F"),
                    Nombre = "MATRIZ",
                    Descripcion = "VENTA AL PÚBLICO EN GENERAL."
                },
                new Almacen
                {
                    AlmacenID = Guid.Parse("BEAD5F60-1270-4281-BBDD-FF83E5147C4C"),
                    Nombre = "ALMACÉN",
                    Descripcion = "PROVEEDOR DE PRODUCTOS DE LA MATRIZ."
                }
            );
        }
    }
}

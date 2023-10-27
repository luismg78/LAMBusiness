namespace LAMBusiness.Contextos.Entities
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Catalogo;

    public class PaqueteConfiguracion : IEntityTypeConfiguration<Paquete>
    {
        public void Configure(EntityTypeBuilder<Paquete> builder)
        {
            builder.HasData(
                //Paquete pieza
                new Paquete { 
                    ProductoID = Guid.Parse("94C079EE-1FBE-4CAE-9A16-443261DD0D60"),
                    PiezaProductoID = Guid.Parse("DE7C7462-69BA-4343-A328-012F48F013AF"),
                    CantidadProductoxPaquete = 12
                },
                //Paquete kilogramo
                new Paquete
                {
                    ProductoID = Guid.Parse("435A7B4D-1347-4282-9B06-3792ED1A99C4"),
                    PiezaProductoID = Guid.Parse("38ABF163-90AD-4D67-9BAB-E5867D2715CF"),
                    CantidadProductoxPaquete = 20
                }
            );
        }
    }
}

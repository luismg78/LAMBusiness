namespace LAMBusiness.Contextos.Entities
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Catalogo;

    public class PuestoConfiguracion : IEntityTypeConfiguration<Puesto>
    {
        public void Configure(EntityTypeBuilder<Puesto> builder)
        {
            builder.HasData(
                new Puesto { 
                    PuestoID = Guid.Parse("D6D5973B-FA59-4B0F-837A-35F83350A63E"),
                    Nombre = "PUESTO",
                    Descripcion = "DESCRIPCIÓN DEL PUESTO"
                }
            );
        }
    }
}

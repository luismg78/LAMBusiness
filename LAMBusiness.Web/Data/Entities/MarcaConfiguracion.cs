namespace LAMBusiness.Web.Data.Entities
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Catalogo;
    using System;

    public class MarcaConfiguracion : IEntityTypeConfiguration<Marca>
    {
        public void Configure(EntityTypeBuilder<Marca> builder)
        {
            builder.HasData(
                //new Marca { 
                //    MarcaID = Guid.Empty,
                //    MarcaNombre = "SIN MARCA",
                //    MarcaDescripcion = "ETIQUETAR PRODUCTO SIN MARCA"
                //},
                new Marca
                {
                    MarcaID = Guid.Parse("620ceb37-d6a5-4649-9c6e-39581858efd2"),
                    MarcaNombre = "SIN MARCA",
                    MarcaDescripcion = "PRODUCTO DE NUEVA CREACIÓN SIN MARCA REGISTRADA."
                }
            );
        }
    }
}

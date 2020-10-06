namespace LAMBusiness.Web.Data.Entities
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Movimiento;

    public class ExistenciaConfiguracion : IEntityTypeConfiguration<Existencia>
    {
        public void Configure(EntityTypeBuilder<Existencia> builder)
        {
            builder.HasData(
                GetExistenciaProductoPieza(),
                GetExistenciaProductoKilogramo()
            );
        }

        private Existencia GetExistenciaProductoPieza()
        {
            return new Existencia
            {
                AlmacenID = Guid.Parse("8706EF28-2EBA-463A-BAB4-62227965F03F"),
                ExistenciaEnAlmacen = 22,
                ExistenciaEnAlmacenMaxima = 30,
                ExistenciaEnAlmacenMinima = 12,
                ExistenciaID = Guid.NewGuid(),
                ProductoID = Guid.Parse("DE7C7462-69BA-4343-A328-012F48F013AF")
            };
        }

        private Existencia GetExistenciaProductoKilogramo()
        {
            return new Existencia
            {
                AlmacenID = Guid.Parse("8706EF28-2EBA-463A-BAB4-62227965F03F"),
                ExistenciaEnAlmacen = (decimal)5.50,
                ExistenciaEnAlmacenMaxima = 15,
                ExistenciaEnAlmacenMinima = 7,
                ExistenciaID = Guid.NewGuid(),
                ProductoID = Guid.Parse("38ABF163-90AD-4D67-9BAB-E5867D2715CF")
            };
        }
    }
}

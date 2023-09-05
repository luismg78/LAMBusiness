namespace LAMBusiness.Web.Data.Entities
{
    using LAMBusiness.Shared.Catalogo;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TasaImpuestoConfiguracion : IEntityTypeConfiguration<TasaImpuesto>
    {
        public void Configure(EntityTypeBuilder<TasaImpuesto> builder)
        {
            builder.HasData(
                new TasaImpuesto { 
                    TasaID = Guid.Parse("ACBB8324-7514-4C38-8354-FA5147FA87E6"),
                    Porcentaje = 0,
                    Nombre = "TASA UNO",
                    Descripcion = "INFORMACIÓN DE APOYO (MODIFICAR INFORMACIÓN)."
                },
                new TasaImpuesto
                {
                    TasaID = Guid.Parse("89E98CD2-85DF-401A-9F1D-308027A75558"),
                    Porcentaje = 16,
                    Nombre = "TASA DOS",
                    Descripcion = "INFORMACIÓN DE APOYO (MODIFICAR INFORMACIÓN)."
                }
            );
        }
    }
}

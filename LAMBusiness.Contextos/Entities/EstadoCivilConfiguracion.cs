namespace LAMBusiness.Contextos.Entities
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Catalogo;
    public class EstadoCivilConfiguracion : IEntityTypeConfiguration<EstadoCivil>
    {
        public void Configure(EntityTypeBuilder<EstadoCivil> builder)
        {
            builder.HasData(
                new EstadoCivil
                {
                    EstadoCivilID = 1,
                    Nombre = "SOLTERO"
                },
                new EstadoCivil
                {
                    EstadoCivilID = 2,
                    Nombre = "CASADO"
                }
            );
        }
    }
}

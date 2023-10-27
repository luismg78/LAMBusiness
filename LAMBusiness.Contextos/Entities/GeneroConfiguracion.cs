namespace LAMBusiness.Contextos.Entities
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Catalogo;

    public class GeneroConfiguracion : IEntityTypeConfiguration<Genero>
    {
        public void Configure(EntityTypeBuilder<Genero> builder)
        {
            builder.HasData(
                new Genero()
                {
                    GeneroID = "F",
                    Nombre = "FEMENINO"
                },
                new Genero()
                {
                    GeneroID = "M",
                    Nombre = "MASCULINO"
                }
            );
        }
    }
}

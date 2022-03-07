namespace LAMBusiness.Web.Data.Entities
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
                    GeneroDescripcion = "FEMENINO"
                },
                new Genero()
                {
                    GeneroID = "M",
                    GeneroDescripcion = "MASCULINO"
                }
            );
        }
    }
}

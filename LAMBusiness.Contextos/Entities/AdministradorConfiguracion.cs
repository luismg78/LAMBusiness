namespace LAMBusiness.Contextos.Entities
{
    using LAMBusiness.Shared.Contacto;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AdministradorConfiguracion : IEntityTypeConfiguration<Administrador>
    {
        public void Configure(EntityTypeBuilder<Administrador> builder)
        {
            builder.HasData(
                new Administrador
                {
                    AdministradorID = "SA",
                    Nombre = "Administrador del Sistema",
                    Descripcion = "Administrador con todos los permisos, incluyendo las configuraciones del sistema."
                },
                new Administrador
                {
                    AdministradorID = "GA",
                    Nombre = "Administrador General",
                    Descripcion = "Administrador con todos los permisos, excluyendo las configuraciones del sistema."
                },
                new Administrador
                {
                    AdministradorID = "NA",
                    Nombre = "No Aplica",
                    Descripcion = "No aplica, los permisos por módulos tienen que ser asignados a cada usuario."
                }
            );
        }
    }
}

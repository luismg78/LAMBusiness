namespace LAMBusiness.Web.Data.Entities
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Contacto;

    public class ColaboradorConfiguracion : IEntityTypeConfiguration<Colaborador>
    {
        public void Configure(EntityTypeBuilder<Colaborador> builder)
        {
            builder.HasData(
                new Colaborador { 
                    ColaboradorID = Guid.NewGuid(),
                    Activo = true,
                    CodigoPostal = 29000,
                    Colonia = "COLONIA",
                    CURP = "CURP781227HCSRNS00",
                    Domicilio = "DOMICILIO",
                    Email = "administrador@lambusiness.com",
                    EstadoCivilID = 2,
                    EstadoNacimientoID = 7,
                    FechaNacimiento = Convert.ToDateTime("1978-12-27"),
                    FechaRegistro = DateTime.Now,
                    GeneroID = "M",
                    MunicipioID = 180,
                    Nombre = "NOMBRE",
                    PrimerApellido = "PRIMERAPELLIDO",
                    PuestoID = Guid.Parse("D6D5973B-FA59-4B0F-837A-35F83350A63E"),
                    SegundoApellido = "SEGUNDOAPELLIDO",
                    Telefono = "1234567890",
                    TelefonoMovil = "0123456789"
                }
            );
        }
    }
}

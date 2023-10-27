namespace LAMBusiness.Contextos.Entities
{
    using LAMBusiness.Shared.Contacto;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasData(
                new Usuario
                {
                    UsuarioID = new("2105AC73-0830-4142-83FF-6FF75B2B8B95"),
                    Activo = true,
                    AdministradorID = "SA",
                    Email = "admin@lambusiness.com",
                    FechaInicio = new DateTime(2022, 12, 27, 10, 0, 0),
                    FechaTermino = new DateTime(9998, 12, 31, 23, 59, 59),
                    FechaUltimoAcceso = DateTime.Now,
                    Nombre = "Administrador",
                    PrimerApellido = "Del Sistema",
                    SegundoApellido = "",
                    TelefonoMovil = "1234567890",
                    CambiarPassword = false,
                    Password = "wyGEi4mhUVKa73+ZQsBETN89Zmn35SOLv0RN51fba4hn7FxWqEDh+PodoKbRrZhzrXCfEJGztLmO0puiay2KWmXecdr5/RWC4k9XgCTAo5MXx2a1MR9CtwaZ7DIRLj69BaiVu3Dvrb1NnogkNOQwKe7Xx9ZjmcQj35xNNteCTH2Qb5RKG5/wR2NDfldNUog033mVZ0bhFBfT8x7VQHxkc6FnTIzbVtCwvm0vjrGiBozISLuDXuU0QJmQwmMuOz+Lwz9L9MuegKx6VfmYIbyZiLOW9+ocplCCXujJbPzwO8d6AeNO2eu7WP54owQOnX2+H7oogLIRdR/JPk4L/nFB0nBECkQSZOZlzbZcGp83ZrA="
                }
            );
        }
    }
}

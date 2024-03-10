using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.Catalogo;

namespace LAMBusiness.Escritorio
{
    public static class Global
    {
        public static Guid? UsuarioId { get; set; }
        public static Guid? VentaId { get; set; }
        public static string? Nombre { get; set; } = string.Empty;
        public static string? PrimerApellido { get; set; } = string.Empty;
        public static string? SegundoApellido { get; set; } = string.Empty;
        public static Almacen Almacen { get; set; }
        public static RazonSocial RazonSocial { get; set; }
        public static Resultado? Resultado { get; set; }
        public static bool AplicacionCerrada { get; set; } = false;
    }
}

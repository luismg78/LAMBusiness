using LAMBusiness.Shared.Aplicacion;

namespace LAMBusiness.Escritorio
{
    public static class Global
    {
        public static Guid? UsuarioId { get; set; }
        public static Guid? VentaId { get; set; }
        public static string? Nombre { get; set; } = string.Empty;
        public static string? PrimerApellido { get; set; } = string.Empty;
        public static string? SegundoApellido { get; set; } = string.Empty;
        public static Resultado? Resultado { get; set; }
        public static bool AplicacionCerrada { get; set; } = false;
    }
}

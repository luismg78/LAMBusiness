namespace LAMBusiness.Shared.Aplicacion
{
    using System;
    using System.Collections.Generic;

    public class Token
    {
        public string SessionID { get; set; }
        public Guid UsuarioID { get; set; }
        public Guid ColaboradorID { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Administrador { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
        public List<Guid> UsuariosModulos { get; set; }
    }
}

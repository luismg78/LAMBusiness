namespace LAMBusiness.Shared.Dashboard
{
    using System;
    using System.Collections.Generic;

    public class Contacto
    {
        public int Clientes { get; set; }
        public int Colaboradores { get; set; }
        public int Proveedores { get; set; }
        public int Usuarios { get; set; }
        public List<Guid> ModulosMenu { get; set; }

    }
}

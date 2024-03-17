namespace LAMBusiness.Shared.Dashboard
{
    using System;
    using System.Collections.Generic;

    public class Movimiento
    {
        public int CorteDeCaja { get; set; }
        public int Entradas { get; set; }
        public int Inventario { get; set; }
        public int RetiroDeCaja { get; set; }
        public int Salidas { get; set; }
        public int Ventas { get; set; }
        public List<Guid> ModulosMenu { get; set; }
    }
}

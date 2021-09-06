namespace LAMBusiness.Shared.Dashboard
{
    using System;
    using System.Collections.Generic;

    public class Catalogo
    {
        public int Almacenes { get; set; }
        public int Generos { get; set; }
        public int Estados { get; set; }
        public int EstadosCiviles { get; set; }
        public int FormasPago { get; set; }
        public int Marcas { get; set; }
        public int Municipios { get; set; }
        public int Productos { get; set; }
        public int Puestos { get; set; }
        public int SalidasTipo { get; set; }
        public int TasasImpuestos { get; set; }
        public int Unidades { get; set; }
        public List<Guid> ModulosMenu { get; set; }
    }
}

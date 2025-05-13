using System;

namespace LAMBusiness.Shared.Aplicacion
{
    public class Filtro
    {
        public string Patron { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Registros { get; set; }
        public int Skip { get; set; }
        public bool PermisoLectura { get; set; }
        public bool PermisoEscritura { get; set; }
        public bool PermisoImprimir { get; set; }
        public int TipoDeMovimiento { get; set; }
    }
    public class Filtro<T>: Filtro
    {
        public T Datos { get; set; }
    }
}

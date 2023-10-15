using System;

namespace LAMBusiness.Shared.Aplicacion
{
    public class Filtro<T>
    {
        public T Datos { get; set; }
        public string Patron { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Registros { get; set; }
        public int Skip { get; set; }
        public bool PermisoLectura { get; set; }
        public bool PermisoEscritura { get; set; }
        public bool PermisoImprimir { get; set; }
    }
}

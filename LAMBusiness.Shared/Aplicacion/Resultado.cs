using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace LAMBusiness.Shared.Aplicacion
{
    public class Resultado<T> : Resultado
    {
        public Resultado()
        {
        }
        public Resultado(T response)
        {
            Datos = response;
        }
        public int TotalDeRegistros { get; set; }
        public int TotalDeRegistrosFiltrados { get; set; }
        public T Datos { get; set; } = default(T)!;
    }

    public class Resultado
    {
        public Resultado()
        {
            Mensajes = new List<string>();
        }
        public bool Error { get; set; }
        public bool Reiniciar { get; set; }

        public string Mensaje
        {
            get
            {
                return string.Join(Environment.NewLine, Mensajes);
            }
            set
            {
                Mensajes.Add(value);
            }
        }
        public string Excepcion { get; set; } = string.Empty;
        public List<string> Mensajes { get; set; }
    }
}


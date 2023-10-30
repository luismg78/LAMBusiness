using System;

namespace LAMBusiness.Shared.Aplicacion
{
    public class Configuracion
    {
        public Guid AplicacionId { get; set; }
        public string CadenaDeConexion { get; set; } = string.Empty;
        public string CadenaDeConexionBitacora { get; set; } = string.Empty;
        //public string CadenaDeConexionDeAutenticacion { get; set; } = string.Empty;
        public string Dominio { get; set; } = "";
        public string UrlCuenta { get; set; } = "";
        public string UrlAPICuenta { get; set; } = "";
        public string UrlCerrarSesion { get; set; } = "";
        public string UrlCDN { get; set; } = "";
        public int RegistrosPorLista { get; set; }
        public bool ModoDesarrollo { get; set; }
        public bool EnMantenimiento { get; set; }

        public Configuracion()
        {
        }

        public Configuracion(Configuracion c)
        {
            AplicacionId = c.AplicacionId;
            CadenaDeConexion = c.CadenaDeConexion;
            CadenaDeConexionBitacora = c.CadenaDeConexionBitacora;
            //CadenaDeConexionDeAutenticacion = c.CadenaDeConexionDeAutenticacion;
            Dominio = c.Dominio;
            UrlCuenta = c.UrlCuenta;
            UrlAPICuenta = c.UrlAPICuenta;
            UrlCerrarSesion = c.UrlCerrarSesion;
            UrlCDN = c.UrlCDN;
            RegistrosPorLista = c.RegistrosPorLista;
            ModoDesarrollo = c.ModoDesarrollo;
            EnMantenimiento = c.EnMantenimiento;
        }
    }
}

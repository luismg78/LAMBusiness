namespace LAMBusiness.Web.Models.ViewModels
{
    using Shared.Aplicacion;

    public class BitacoraExcepcionViewModel: Bitacora
    {
        public string Excepcion { get; set; }
        public string ErrorAlGuardarBitacoraDB { get; set; }
    }
}

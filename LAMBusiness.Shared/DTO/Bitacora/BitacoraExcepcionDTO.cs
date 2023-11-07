namespace LAMBusiness.Shared.DTO.Bitacora
{
    using Shared.Aplicacion;

    public class BitacoraExcepcionDTO : Bitacora
    {
        public string Excepcion { get; set; }
        public string ErrorAlGuardarBitacoraDB { get; set; }
    }
}

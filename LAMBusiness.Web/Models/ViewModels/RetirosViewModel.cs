namespace LAMBusiness.Web.Models.ViewModels
{
    using System;

    public class RetirosViewModel
    {
        public Guid UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal Importe { get; set; }
        public Guid? VentaPendiente { get; set; }
    }
}

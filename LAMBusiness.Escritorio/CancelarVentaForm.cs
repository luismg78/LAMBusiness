using LAMBusiness.Backend;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;

namespace LAMBusiness.Escritorio
{
    public partial class CancelarVentaForm : Form
    {
        private readonly Configuracion _configuracion;
        private readonly Guid _usuarioId;
        private readonly Guid _ventaId;
        private Resultado? _resultado;

        public CancelarVentaForm(Configuracion configuracion, Guid ventaId)
        {
            InitializeComponent();
            _configuracion = configuracion;
            _usuarioId = (Guid)Global.UsuarioId!;
            _ventaId = ventaId;
            _resultado = null;
        }

        private async void CancelarVentaButton_Click(object sender, EventArgs e)
        {
            using var contexto = new DataContext(_configuracion);
            Ventas ventas = new(contexto);
            var ventaCancelada = await ventas.CancelarVenta(_ventaId, _usuarioId);
            //await BitacoraAsync("CancelarVenta", ventaCancelada.Datos, _usuarioId, ventaCancelada.Mensaje);
            _resultado = new Resultado()
            {
                Error = ventaCancelada.Error,
                Mensaje = ventaCancelada.Mensaje
            };
            Close();
        }

        private async void GuardarVentaButton_Click(object sender, EventArgs e)
        {
            using var contexto = new DataContext(_configuracion);
            Ventas ventas = new(contexto);
            var guardar = await ventas.Agregar(_ventaId, _usuarioId);
            _resultado = new Resultado()
            {
                Error = guardar.Error,
                Mensaje = guardar.Mensaje
            };
            Close();
        }

        private void CancelarVentaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.Resultado = _resultado;
        }

        //private async Task BitacoraAsync(string accion, VentasDTO venta, string excepcion = "")
        //{
        //    BitacoraContext bitacoraContexto = new(_configuracion.CadenaDeConexionBitacora);
        //    var _bitacora = new Bitacoras(bitacoraContexto);

        //    await _bitacora.AgregarAsync(token, accion, Guid.Empty,
        //        venta, venta.VentaID.ToString(), directorioBitacora, excepcion);
        //}
        //private async Task BitacoraAsync(string accion, VentaCanceladaDTO ventaCancelada, Guid usuarioId, string excepcion = "")
        //{
        //    string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

        //    await _getHelper.SetBitacoraAsync(token, accion, moduloId,
        //        ventaCancelada, usuarioId.ToString(), directorioBitacora, excepcion);
        //}
    }
}

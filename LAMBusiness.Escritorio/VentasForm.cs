using LAMBusiness.Backend;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.Contacto;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace LAMBusiness.Escritorio
{
    public partial class VentasForm : Form
    {
        #region Variables enumerables
        enum Proceso
        {
            Capturar,
            Aplicar,
            Buscar,
            Retirar,
            CorteDeCaja
        }
        #endregion

        #region Variables globales del formulario
        private readonly DataContext _contexto;
        private readonly Ventas _ventas;
        private readonly Productos _productos;
        private readonly Guid _razonSocialId = new("E9212EB2-697A-4358-9CDE-9123B66676EB");
        private readonly Configuracion _configuracion;
        private decimal _cantidad;
        private decimal _pago;
        private Guid _usuarioId;
        private Proceso _proceso = Proceso.Capturar;
        public Guid _ventaId;
        #endregion

        #region Constructor
        public VentasForm(Configuracion configuracion)
        {
            InitializeComponent();
            _contexto = new(configuracion);
            _ventas = new Ventas(_contexto);
            _productos = new Productos(_contexto);
            _configuracion = configuracion;
            _cantidad = 1;
        }
        #endregion

        #region Inicio y cierre del formulario
        private void VentasForm_Load(object sender, EventArgs e)
        {
            if (Global.UsuarioId == null || Global.UsuarioId == Guid.Empty)
            {
                IniciarSesionForm form = new(_configuracion);
                Hide();
                form.Show();
            }

            WindowState = FormWindowState.Maximized;
            _usuarioId = (Guid)Global.UsuarioId!;
            Notificar();
            IniciarVenta();
        }
        private void VentasForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Global.AplicacionCerrada)
            {
                var resultado = MessageBox.Show("¿Desea cerrar la aplicación?", "Cerrar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    Global.AplicacionCerrada = true;
                    Application.Exit();
                }
                else
                    e.Cancel = true;
            }
        }
        #endregion

        #region Funcionalidad de código (textbox)
        private async void CodigoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Resultado resultado;
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    switch (_proceso)
                    {
                        case Proceso.Capturar:
                            resultado = await ObtenerProductoPorCodigoAsync();
                            if (resultado.Error)
                                Notificar(resultado.Mensaje);
                            break;
                        case Proceso.Buscar:
                            resultado = await BuscarProductoPorCodigoAsync();
                            if (resultado.Error)
                                Notificar(resultado.Mensaje);
                            break;
                        case Proceso.Aplicar:
                            resultado = await AplicarVentaAsync();
                            if (resultado.Error)
                                Notificar(resultado.Mensaje);
                            break;
                    }
                    break;
                case Keys.Escape:
                    switch (_proceso)
                    {
                        case Proceso.Capturar:
                            CodigoTextBox.Text = string.Empty;
                            break;
                        case Proceso.Buscar:
                        case Proceso.Aplicar:
                            IniciarCaptura();
                            break;
                    }
                    break;
                case Keys.Multiply:
                case Keys.Oemplus:
                    resultado = ValidarCantidad();
                    if (resultado.Error)
                        Notificar(resultado.Mensaje);
                    CodigoTextBox.Text = string.Empty;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.F1:
                    //buscar
                    //var ayuda = new VentaAyudaForm();
                    //ayuda.ShowDialog();
                    break;
                case Keys.F2:
                    IniciarBuscar();
                    break;
                case Keys.F5:
                    IniciarCobro();
                    break;
                case Keys.F7:
                    CancelarVentaModal();
                    break;
                case Keys.F8:
                    RecuperarVentasModal();
                    break;
                case Keys.F11:
                    Notificar("Retiro de caja [pendiente]");
                    break;
                case Keys.F12:
                    Notificar("Corte de caja [pendiente]");
                    break;
            }
        }
        #endregion

        #region Funcionalidad del datagrid
        private void ProductosDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    DataGridView row = (DataGridView)sender;
                    int i = row.CurrentRow.Index;
                    decimal cantidad = (decimal)ProductosDataGridView.Rows[i].Cells[0].Value * -1;
                    string pregunta = "¿Desea eliminar el detalle del producto?";
                    Color color = Color.Red;
                    if (cantidad > 0)
                    {
                        color = Color.Black;
                        pregunta = "¿Desea restablecer el detalle del producto?";
                    }

                    if (MessageBox.Show(pregunta, "Ventas", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        ProductosDataGridView.Rows[i].Cells[0].Value = cantidad;
                        ProductosDataGridView.Rows[i].Cells[4].Value = (decimal)ProductosDataGridView.Rows[i].Cells[3].Value * cantidad;
                        ProductosDataGridView.Rows[i].DefaultCellStyle.ForeColor = color;
                        ObtenerTotal();
                    }
                    CodigoTextBox.Focus();
                    break;
                case Keys.Escape:
                    CodigoTextBox.Text = string.Empty;
                    CodigoTextBox.Focus();
                    break;
            }
        }
        #endregion

        #region Botones
        private void BuscarButton_Click(object sender, EventArgs e)
        {
            IniciarBuscar();
        }

        private void CerrarButton_Click(object sender, EventArgs e)
        {
            if (!Global.AplicacionCerrada)
            {
                var resultado = MessageBox.Show("¿Desea cerrar la aplicación?", "Cerrar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    Global.AplicacionCerrada = true;
                    Application.Exit();
                }
            }
        }

        private void ConfiguracionButtonBgColor(string proceso)
        {
            Notificar();
            BuscarButton.BackColor = Color.White;
            BuscarButton.ForeColor = Color.Black;
            CancelarButton.BackColor = Color.White;
            CancelarButton.ForeColor = Color.Black;
            CobrarButton.BackColor = Color.White;
            CobrarButton.ForeColor = Color.Black;
            CorteDeCajaButton.BackColor = Color.White;
            CorteDeCajaButton.ForeColor = Color.Black;
            RecuperarButton.BackColor = Color.White;
            RecuperarButton.ForeColor = Color.Black;
            RetirarEfectivoButton.BackColor = Color.White;
            RetirarEfectivoButton.ForeColor = Color.Black;
            VentasButton.BackColor = Color.White;
            VentasButton.ForeColor = Color.Black;
            IconoPictureBox.Image = Properties.Resources.codigodebarras;
            switch (proceso)
            {
                case "buscar":
                    BuscarButton.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(65)))), ((int)(((byte)(82)))));
                    BuscarButton.ForeColor = Color.White;
                    IconoPictureBox.Image = Properties.Resources.buscar;
                    break;
                case "cancelar":
                    CancelarButton.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(65)))), ((int)(((byte)(82)))));
                    CancelarButton.ForeColor = Color.White;
                    break;
                case "cobrar":
                    CobrarButton.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(65)))), ((int)(((byte)(82)))));
                    CobrarButton.ForeColor = Color.White;
                    IconoPictureBox.Image = Properties.Resources.signopesos;
                    break;
                case "cortedecaja":
                    CorteDeCajaButton.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(65)))), ((int)(((byte)(82)))));
                    CorteDeCajaButton.ForeColor = Color.White;
                    break;
                case "recuperar":
                    RecuperarButton.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(65)))), ((int)(((byte)(82)))));
                    RecuperarButton.ForeColor = Color.White;
                    break;
                case "retirar":
                    RetirarEfectivoButton.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(65)))), ((int)(((byte)(82)))));
                    RetirarEfectivoButton.ForeColor = Color.White;
                    break;
                case "ventas":
                    VentasButton.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(65)))), ((int)(((byte)(82)))));
                    VentasButton.ForeColor = Color.White;
                    break;
            }
        }

        private void CobrarButton_Click(object sender, EventArgs e)
        {
            IniciarCobro();
        }

        private void CancelarButton_Click(object sender, EventArgs e)
        {
            CancelarVentaModal();
        }

        private void RecuperarButton_Click(object sender, EventArgs e)
        {
            RecuperarVentasModal();
        }

        private void VentasButton_Click(object sender, EventArgs e)
        {
            IniciarCaptura();
        }
        #endregion

        #region Ventas
        private async Task<Resultado> AplicarVentaAsync()
        {
            Resultado<decimal> resultado = new();
            resultado = ValidarImporte();
            if (resultado.Error)
                return resultado;

            decimal importe = resultado.Datos;
            var venta = await _ventas.Aplicar(_ventaId, _usuarioId, importe);
            if (venta.Error)
            {
                resultado.Error = true;
                resultado.Mensaje = venta.Mensaje;
                return resultado;
            }

            ObtenerCambio();
            IniciarCaptura();
            return resultado;
        }

        public async Task<Resultado> BuscarProductoPorCodigoAsync()
        {
            Notificar();
            Resultado resultado = new();
            BuscarForm form = new(_contexto);
            var productos = await form.ObtenerProductos(CodigoTextBox.Text);
            if (productos != null && productos.Any())
            {
                form.productos = productos;
                form.ShowDialog();
                if (!string.IsNullOrEmpty(form.Codigo))
                {
                    CodigoTextBox.Text = form.Codigo;
                    resultado = await ObtenerProductoPorCodigoAsync();
                    if (resultado.Error)
                        Notificar(resultado.Mensaje);
                    else
                        IniciarCaptura();
                }
            }
            else
            {
                resultado.Error = true;
                resultado.Mensaje = "Producto inexistente";
            }

            CodigoTextBox.Text = "";
            return resultado;
        }

        public void CancelarVentaModal()
        {
            bool ok = HayRegistrosDeVentasPorAplicar();
            if (ok)
            {
                ConfiguracionButtonBgColor("cancelar");
                var form = new CancelarVentaForm(_configuracion, _ventaId);
                form.ShowDialog();
                var resultado = Global.Resultado;
                if (resultado != null)
                {
                    if (resultado.Error)
                        Notificar(resultado.Mensaje);
                    else
                    {
                        IniciarVenta();
                        IniciarCaptura();
                    }
                }
            }
            else
            {
                Notificar("Opción no aprobada, no hay movimientos en la lista.");
            }
        }

        public async Task<Resultado> ObtenerProductoPorCodigoAsync()
        {
            var resultado = await _ventas.ObtenerProducto(_ventaId, _usuarioId, CodigoTextBox.Text, _cantidad);
            if (resultado.Error)
            {
                switch (resultado.Mensaje.Trim().ToLower())
                {
                    case "buscarproducto":
                        var buscar = await BuscarProductoPorCodigoAsync();
                        resultado.Error = buscar.Error;
                        resultado.Mensaje = buscar.Mensaje;
                        break;
                    case "reiniciar":
                        Notificar("Identificador de la venta incorrecto.");
                        //reiniciar venta
                        break;
                    default:
                        Notificar(resultado.Mensaje);
                        //reiniciar venta
                        break;
                }
                return resultado;
            }

            var producto = resultado.Datos;
            ProductosDataGridView.Rows.Add(_cantidad, producto.Productos.Codigo, producto.Productos.Nombre, producto.Productos.PrecioVenta, producto.Productos.PrecioVenta * _cantidad);
            ProductosDataGridView.Rows[^1].Selected = true;
            ProductosDataGridView.FirstDisplayedScrollingRowIndex = ProductosDataGridView.Rows.Count - 1;
            CobrarButton.Enabled = true;
            CancelarButton.Enabled = true;
            RecuperarButton.Enabled = true;
            CodigoTextBox.Text = string.Empty;
            _cantidad = 1;
            ObtenerTotal();

            return resultado;
        }

        private async void RecuperarVentas(Guid id)
        {
            _ventaId = id;
            var resultado = await _ventas.RecuperarVentaPorId(id, _usuarioId);
            if (!resultado.Error)
            {
                var ventas = resultado.Datos;
                if (ventas.VentasNoAplicadasDetalle != null && ventas.VentasNoAplicadasDetalle.Any())
                {
                    foreach (var venta in ventas.VentasNoAplicadasDetalle)
                    {
                        ProductosDataGridView.Rows.Add(venta.Cantidad, venta.Productos.Codigo, venta.Productos.Nombre, venta.Productos.PrecioVenta, venta.Productos.PrecioVenta * venta.Cantidad);
                        ProductosDataGridView.Rows[^1].Selected = true;
                        ProductosDataGridView.FirstDisplayedScrollingRowIndex = ProductosDataGridView.Rows.Count - 1;
                    }
                    CobrarButton.Enabled = true;
                }
            }
            else
            {
                Notificar(resultado.Mensaje);
            }

            _cantidad = 1;
            ObtenerTotal();
            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.Focus();
        }

        public async void RecuperarVentasModal()
        {
            bool ok = HayRegistrosDeVentasPorAplicar();
            if (ok)
            {
                Notificar("Opción no aprobada, venta en proceso.");
            }
            else
            {
                var hayVentasNoAplicadas = await (from v in _contexto.VentasNoAplicadas
                                                  join d in _contexto.VentasNoAplicadasDetalle
                                                  on v.VentaNoAplicadaID equals d.VentaNoAplicadaID
                                                  where v.UsuarioID == _usuarioId
                                                  select v).AnyAsync();
                if (hayVentasNoAplicadas)
                {
                    ConfiguracionButtonBgColor("recuperar");
                    var form = new RecuperarVentasForm(_configuracion, _ventaId);
                    form.ShowDialog();
                    _ventaId = (Guid)Global.VentaId!;
                    if (_ventaId != Guid.Empty)
                        RecuperarVentas(_ventaId);
                    IniciarCaptura();
                }
                else
                    Notificar("No existe registro de ventas pendientes por aplicar.");
            }
        }
        #endregion

        #region Reseteo
        private void IniciarBuscar()
        {
            ConfiguracionButtonBgColor("buscar");
            _cantidad = 1;
            _proceso = Proceso.Buscar;
            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.Focus();
        }
        private async void IniciarVenta()
        {
            var resultado = await _ventas.Inicializar((Guid)Global.UsuarioId!);
            if (resultado.Error)
            {
                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                IniciarSesionForm form = new(_configuracion);
                Hide();
                form.Show();
            }

            var ventaNoAplicada = resultado.Datos;

            CancelarButton.Enabled = false;
            RecuperarButton.Enabled = false;
            if (ventaNoAplicada.TotalDeRegistrosPendientes > 0)
            {
                CancelarButton.Enabled = true;
                RecuperarButton.Enabled = true;
            }

            RetirarEfectivoButton.Enabled = false;
            CorteDeCajaButton.Enabled = false;
            if (ventaNoAplicada.HayVentasPorCerrar)
            {
                RetirarEfectivoButton.Enabled = true;
                CorteDeCajaButton.Enabled = true;
            }

            CobrarButton.Enabled = false;

            _cantidad = 1;
            _pago = 0;
            _proceso = Proceso.Capturar;
            _ventaId = ventaNoAplicada.VentaNoAplicadaID;
            ProductosDataGridView.Rows.Clear();
            IconoPictureBox.Image = Properties.Resources.codigodebarras;
            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.Focus();
        }
        private void IniciarCaptura()
        {
            ConfiguracionButtonBgColor("ventas");
            _cantidad = 1;
            _proceso = Proceso.Capturar;
            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.Focus();
        }
        private void IniciarCobro()
        {
            bool ok = HayRegistrosDeVentasPorAplicar();
            if (ok)
            {
                _pago = 0;
                _proceso = Proceso.Aplicar;
                ConfiguracionButtonBgColor("cobrar");
                ObtenerTotal();
                CodigoTextBox.Text = string.Empty;
                CodigoTextBox.Focus();
            }
            else
            {
                Notificar("Operación no aprobada, agregue al menos un registro en la lista.");
            }
        }
        #endregion

        #region Importes totales de la venta
        private decimal CalcularTotal()
        {
            decimal total = 0;
            for (var i = 0; i < ProductosDataGridView.Rows.Count; i++)
            {
                decimal importe = Convert.ToDecimal(ProductosDataGridView.Rows[i].Cells[4].Value);
                total += importe > 0 ? importe : 0;
            }

            return total;
        }
        private void ObtenerTotal()
        {
            decimal total = CalcularTotal();
            TotalLabel.Text = $"Total {total:$0.00}";
        }
        private void ObtenerCambio()
        {
            decimal total = CalcularTotal();
            _pago += Convert.ToDecimal(CodigoTextBox.Text);
            decimal diferencia = _pago - total;
            if (diferencia < 0)
            {
                TotalLabel.Text = $"Resta {Math.Abs(diferencia):$0.00}";
            }
            else
            {
                IniciarVenta();
                TotalLabel.Text = $"Cambio {diferencia:$0.00}";
            }
            CodigoTextBox.Text = string.Empty;
        }
        #endregion

        #region Mensajes
        private void Notificar(string mensaje = "")
        {
            //NotificacionLabel.Text = mensaje;
            NotificacionToolStripStatusLabel.Text = mensaje;
        }
        #endregion

        #region Validaciones
        private Resultado ValidarCantidad()
        {
            Resultado resultado = new();
            _cantidad = 1;

            if (string.IsNullOrEmpty(CodigoTextBox.Text))
                return resultado;

            if (!Regex.IsMatch(CodigoTextBox.Text, @"^\d+(?:\.\d+)?$"))
            {
                resultado.Error = true;
                resultado.Mensaje = $"Cantidad ({CodigoTextBox.Text}) incorrecta";
                return resultado;
            }

            _cantidad = Convert.ToDecimal(CodigoTextBox.Text);

            if (_cantidad <= 0)
            {
                resultado.Error = true;
                resultado.Mensaje = $"La Cantidad no puede ser igual o menor a cero.";
            }

            return resultado;
        }

        private Resultado<decimal> ValidarImporte()
        {
            Resultado<decimal> resultado = new();

            if (string.IsNullOrEmpty(CodigoTextBox.Text))
            {
                resultado.Error = true;
                resultado.Mensaje = "Importe incorrecto.";
                return resultado;
            }

            if (!Regex.IsMatch(CodigoTextBox.Text, @"^\d+(?:\.\d+)?$"))
            {
                resultado.Error = true;
                resultado.Mensaje = $"Importe ({CodigoTextBox.Text}) incorrecto.";
                return resultado;
            }

            resultado.Datos = Convert.ToDecimal(CodigoTextBox.Text);
            return resultado;
        }

        private bool HayRegistrosDeVentasPorAplicar()
        {
            if (ProductosDataGridView == null)
                return false;

            return ProductosDataGridView.RowCount > 0;
        }
        #endregion
    }
}

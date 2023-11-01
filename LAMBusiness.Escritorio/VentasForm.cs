using DocumentFormat.OpenXml.Vml.Spreadsheet;
using LAMBusiness.Backend;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
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
            Retirar,
            CorteDeCaja
        }
        #endregion

        #region Variables globales del formulario
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
            DataContext contexto = new(configuracion);
            _ventas = new Ventas(contexto);
            _productos = new Productos(contexto);
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
                                MensajeDeError(resultado.Mensaje);
                            break;
                        case Proceso.Aplicar:
                            resultado = await AplicarVentaAsync();
                            if (resultado.Error)
                                MensajeDeError(resultado.Mensaje);
                            break;
                    }
                    break;
                case Keys.Escape:
                    switch (_proceso)
                    {
                        case Proceso.Capturar:
                            CodigoTextBox.Text = string.Empty;
                            break;
                        case Proceso.Aplicar:
                            IniciarCaptura();
                            break;
                    }
                    break;
                case Keys.Multiply:
                case Keys.Oemplus:
                    resultado = ValidarCantidad();
                    if (resultado.Error)
                        MensajeDeError(resultado.Mensaje);
                    CodigoTextBox.Text = string.Empty;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.F1:
                    //buscar
                    //var ayuda = new VentaAyudaForm();
                    //ayuda.ShowDialog();
                    break;
                case Keys.F3:
                    CancelarVentaModal();
                    break;
                case Keys.F4:
                    RecuperarVentasModal();
                    break;
                case Keys.F5:
                    IniciarCobro();
                    break;
                case Keys.F7:
                    MessageBox.Show("F7 - retiro de caja");
                    break;
                case Keys.F8:
                    MessageBox.Show("F8 - Corte de caja");
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
        private void ConfiguracionButtonBgColor(string proceso)
        {
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
            switch(proceso)
            {
                case "buscar":
                    BuscarButton.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(65)))), ((int)(((byte)(82)))));
                    BuscarButton.ForeColor = Color.White;
                    break;
                case "cancelar":
                    CancelarButton.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(65)))), ((int)(((byte)(82)))));
                    CancelarButton.ForeColor = Color.White;
                    break;
                case "cobrar":
                    CobrarButton.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(65)))), ((int)(((byte)(82)))));
                    CobrarButton.ForeColor = Color.White;
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

        private void BuscarButton_Click(object sender, EventArgs e)
        {
            ConfiguracionButtonBgColor("buscar");
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
            ConfiguracionButtonBgColor("ventas");
        }
        #endregion

        #region Ventas
        private async Task<Resultado> AplicarVentaAsync()
        {
            Resultado resultado = new();
            resultado = ValidarCantidad();
            if (!resultado.Error)
                ObtenerCambio();

            return resultado;
        }

        public async Task<Resultado> ObtenerProductoPorCodigoAsync()
        {
            var resultado = await _ventas.ObtenerProducto(_ventaId, _usuarioId, CodigoTextBox.Text, _cantidad);

            if (resultado.Error)
            {
                switch (resultado.Mensaje.Trim().ToLower())
                {
                    case "buscarproducto":
                    //buscar producto
                    case "reiniciar":
                        MensajeDeError("Identificador de la venta incorrecto.");
                        //reiniciar venta
                        break;
                    default:
                        MensajeDeError(resultado.Mensaje);
                        //reiniciar venta
                        break;
                }
            }

            var producto = resultado.Datos;
            ProductosDataGridView.Rows.Add(_cantidad, producto.Productos.Codigo, producto.Productos.Nombre, producto.Productos.PrecioVenta, producto.Productos.PrecioVenta * _cantidad);
            ProductosDataGridView.Rows[^1].Selected = true;
            ProductosDataGridView.FirstDisplayedScrollingRowIndex = ProductosDataGridView.Rows.Count - 1;
            CobrarButton.Enabled = true;
            CodigoTextBox.Text = string.Empty;
            _cantidad = 1;
            ObtenerTotal();

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
                        MensajeDeError(resultado.Mensaje);
                    else
                        IniciarVenta();
                }
            }
            else
            {
                Notificar("Opción no aprobada, no hay movimientos en la lista.");
            }
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
                MensajeDeError(resultado.Mensaje);
            }

            _cantidad = 1;
            ObtenerTotal();
            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.Focus();
        }

        public void RecuperarVentasModal()
        {
            bool ok = HayRegistrosDeVentasPorAplicar();
            if (ok)
            {
                Notificar("Opción no aprobada, venta en proceso.");
            }
            else
            {
                var form = new RecuperarVentasForm(_configuracion, _ventaId);
                form.ShowDialog();
                _ventaId = (Guid)Global.VentaId!;
                RecuperarVentas(_ventaId);
            }
        }

        #endregion

        #region Reseteo
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

            RecuperarButton.Enabled = false;
            if (ventaNoAplicada.TotalDeRegistrosPendientes > 0)
                RecuperarButton.Enabled = true;

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
            VentaTotalLabel.Text = "$0.00";
            IconoPictureBox.Image = Properties.Resources.codigodebarras;
            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.Focus();
        }
        private void IniciarCaptura()
        {
            _cantidad = 1;
            _proceso = Proceso.Capturar;
            IconoPictureBox.Image = Properties.Resources.codigodebarras;
            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.Focus();
        }
        private void IniciarCobro()
        {
            _pago = 0;
            _proceso = Proceso.Aplicar;
            ObtenerTotal();
            IconoPictureBox.Image = Properties.Resources.signopesos;
            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.Focus();
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
            VentaTotalLabel.Text = $"{total:$0.00}";
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
        private static void MensajeDeError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void Notificar(string mensaje = "")
        {
            NotificacionLabel.Text = mensaje;
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

        private bool HayRegistrosDeVentasPorAplicar()
        {
            if (ProductosDataGridView == null)
                return false;

            return ProductosDataGridView.RowCount > 0;
        }
        #endregion
    }
}

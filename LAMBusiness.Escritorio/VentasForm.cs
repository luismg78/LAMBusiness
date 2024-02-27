using DocumentFormat.OpenXml.Drawing.Diagrams;
using LAMBusiness.Backend;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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

        public VentasForm()
        {
        }
        #endregion

        #region Inicio y cierre del formulario
        private async void VentasForm_Load(object sender, EventArgs e)
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
            await IniciarVenta(true);
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
                        case Proceso.Retirar:
                            resultado = AgregarImporteDeRetiroAsync();
                            if (resultado.Error)
                                Notificar(resultado.Mensaje);
                            break;
                        case Proceso.CorteDeCaja:
                            resultado = await AplicarCorteDeCaja();
                            if (resultado.Error)
                                Notificar(resultado.Mensaje);
                            break;
                    }
                    break;
                case Keys.Escape:
                    TotalLabel.ForeColor = Color.Black;
                    switch (_proceso)
                    {
                        case Proceso.Capturar:
                            CodigoTextBox.Text = string.Empty;
                            CodigoTextBox.PasswordChar = (char)0;
                            break;
                        case Proceso.Buscar:
                            await IniciarVenta();
                            break;
                        case Proceso.CorteDeCaja:
                            await IniciarVenta(true);
                            break;
                        case Proceso.Aplicar:
                            await IniciarVenta();
                            break;
                        case Proceso.Retirar:
                            await AplicarRetiroDeCaja();
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
                    if (BuscarButton.Enabled)
                        IniciarBuscar();
                    break;
                case Keys.F5:
                    if (CobrarButton.Enabled)
                        IniciarCobro();
                    break;
                case Keys.F7:
                    if (CancelarButton.Enabled)
                        await CancelarVentaModal();
                    break;
                case Keys.F8:
                    if (RecuperarButton.Enabled)
                        await RecuperarVentasModal();
                    break;
                case Keys.F11:
                    if (RetirarEfectivoButton.Enabled)
                        IniciarRetiroDeCaja();
                    break;
                case Keys.F12:
                    if (CorteDeCajaButton.Enabled)
                        IniciarCorteDeCaja();
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
                    CodigoTextBox.PasswordChar = (char)0;
                    CodigoTextBox.Focus();
                    break;
            }
        }

        private void ProductosDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            MessageBox.Show("Removido");
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
                    IconoPictureBox.Image = Properties.Resources.retirodecaja;
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

        private async void CancelarButton_Click(object sender, EventArgs e)
        {
            await CancelarVentaModal();
        }

        private void CorteDeCajaButton_Click(object sender, EventArgs e)
        {
            IniciarCorteDeCaja();
        }

        private async void RecuperarButton_Click(object sender, EventArgs e)
        {
            await RecuperarVentasModal();
        }

        private void RetirarEfectivoButton_Click(object sender, EventArgs e)
        {
            IniciarRetiroDeCaja();
        }

        private async void VentasButton_Click(object sender, EventArgs e)
        {
            switch (_proceso)
            {
                case Proceso.Retirar:
                    await AplicarRetiroDeCaja();
                    break;
                default:
                    await IniciarVenta();
                    break;
            }
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

            await ObtenerCambio();
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
                        await IniciarVenta();
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

        public async Task CancelarVentaModal()
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
                        await IniciarVenta(true);
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

        private async Task RecuperarVentas(Guid id)
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
            CodigoTextBox.PasswordChar = (char)0;
            CodigoTextBox.Focus();
        }

        public async Task RecuperarVentasModal()
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
                        await RecuperarVentas(_ventaId);
                    await IniciarVenta();
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
            CodigoTextBox.PasswordChar = (char)0;
            CodigoTextBox.Focus();
        }
        private async Task IniciarVenta(bool nuevaVenta = false)
        {
            var resultado = await _ventas.Inicializar((Guid)Global.UsuarioId!, nuevaVenta);
            if (resultado.Error)
            {
                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                IniciarSesionForm form = new(_configuracion);
                Hide();
                form.Show();
            }
            else
            {
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
                if (HayRegistrosDeVentasPorAplicar())
                    CobrarButton.Enabled = true;

                BuscarButton.Enabled = true;
                VentasButton.Enabled = true;

                _cantidad = 1;
                _pago = 0;
                _proceso = Proceso.Capturar;
                ConfiguracionButtonBgColor("ventas");
                IconoPictureBox.Image = Properties.Resources.codigodebarras;
                if (nuevaVenta)
                {
                    _ventaId = ventaNoAplicada.VentaNoAplicadaID;
                    ProductosDataGridView.Rows.Clear();
                }
                ObtenerTotal();
                CodigoTextBox.Text = string.Empty;
                CodigoTextBox.PasswordChar = (char)0;
                CodigoTextBox.Focus();

            }
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
                BuscarButton.Enabled = false;
                CobrarButton.Enabled = false;
                CancelarButton.Enabled = false;
                RecuperarButton.Enabled = false;
                RetirarEfectivoButton.Enabled = false;
                CorteDeCajaButton.Enabled = false;
                VentasButton.Enabled = true;
                CodigoTextBox.Text = string.Empty;
                CodigoTextBox.PasswordChar = (char)0;
                CodigoTextBox.Focus();
            }
            else
            {
                Notificar("Operación no aprobada, agregue al menos un registro en la lista.");
            }
        }
        private void IniciarCorteDeCaja()
        {
            bool ok = HayRegistrosDeVentasPorAplicar();
            if (ok)
            {
                Notificar("Opción no aprobada, movimiento en proceso.");
            }
            else
            {
                _pago = 0;
                _proceso = Proceso.CorteDeCaja;
                ConfiguracionButtonBgColor("cortedecaja");
                ObtenerTotal();
                BuscarButton.Enabled = false;
                CobrarButton.Enabled = false;
                CancelarButton.Enabled = false;
                RecuperarButton.Enabled = false;
                RetirarEfectivoButton.Enabled = false;
                CorteDeCajaButton.Enabled = true;
                VentasButton.Enabled = true;
                CodigoTextBox.Text = string.Empty;
                CodigoTextBox.PasswordChar = '*';
                CodigoTextBox.Focus();
            }
        }
        private void IniciarRetiroDeCaja()
        {
            bool ok = HayRegistrosDeVentasPorAplicar();
            if (ok)
            {
                Notificar("Opción no aprobada, movimiento en proceso.");
            }
            else
            {
                _pago = 0;
                _proceso = Proceso.Retirar;
                ConfiguracionButtonBgColor("retirar");
                ObtenerTotal();
                BuscarButton.Enabled = false;
                CobrarButton.Enabled = false;
                CancelarButton.Enabled = false;
                RecuperarButton.Enabled = false;
                RetirarEfectivoButton.Enabled = true;
                CorteDeCajaButton.Enabled = false;
                VentasButton.Enabled = true;
                CodigoTextBox.Text = string.Empty;
                CodigoTextBox.PasswordChar = (char)0;
                CodigoTextBox.Focus();
            }
        }
        #endregion

        #region Corte de cajas
        public async Task<Resultado> AplicarCorteDeCaja()
        {
            Sesiones sesion = new(_contexto);
            var pwd = Sesiones.GenerateSHA512String(CodigoTextBox.Text);
            Resultado resultado = await sesion.ValidarContraseñaPorUsuarioIdAsync(_usuarioId, pwd);
            if (resultado.Error)
                return resultado;

            var resultadoCorteDeCaja = await _ventas.CerrarVentas(_usuarioId);
            if (resultadoCorteDeCaja.Error)
            {
                resultado.Error = true;
                resultado.Mensaje = resultadoCorteDeCaja.Mensaje;
                return resultado;
            }

            var corte = resultadoCorteDeCaja.Datos;

            ProductosDataGridView.Rows.Add(1, "", "Importe del sistema", "", $"{corte.ImporteDelSistema:0.00}");
            //if (corte.ImporteDelSistemaDetalle != null && corte.ImporteDelSistemaDetalle.Any())
            //{
            //    foreach (var item in corte.ImporteDelSistemaDetalle)
            //    {
            //        ProductosDataGridView.Rows.Add(1, "", item.FormaDePago, "", item.Importe);
            //    }
            //}
            ProductosDataGridView.Rows.Add(1, "", "Importe del usuario (retiros)", "", $"{corte.ImporteDelUsuario:0.00}");
            var diferencia = corte.ImporteDelSistema - corte.ImporteDelUsuario;
            if (diferencia > 0)
            {
                TotalLabel.Text = $"Faltante {diferencia:$0.00}";
                TotalLabel.ForeColor = Color.Red;
            }
            else if (diferencia < 0)
            {
                TotalLabel.Text = $"Sobrante {Math.Abs(diferencia):$0.00}";
                TotalLabel.ForeColor = Color.Orange;
            }
            else
            {
                TotalLabel.Text = $"Correcto {diferencia:$0.00}";
                TotalLabel.ForeColor = Color.Green;
            }
            ProductosDataGridView.Rows[^1].Selected = true;
            ProductosDataGridView.FirstDisplayedScrollingRowIndex = ProductosDataGridView.Rows.Count - 1;
            _cantidad = 1;
            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.Focus();
            return resultado;
        }

        #endregion

        #region Retiro de cajas
        public async Task AplicarRetiroDeCaja()
        {
            bool ok = HayRegistrosDeVentasPorAplicar();
            if (ok)
            {
                var r = MessageBox.Show("Desea guardar el retiro de caja", "Retiro de caja", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (r == DialogResult.Yes)
                {
                    decimal importe = CalcularTotal();
                    var retiro = await _ventas.RetirarEfectivoDeCaja(importe, _usuarioId);
                    if (!retiro.Error)
                        await IniciarVenta(true);
                    Notificar(retiro.Mensaje);
                }
                else if (r == DialogResult.No)
                    await IniciarVenta(true);
                else
                {
                    _cantidad = 1;
                    CodigoTextBox.Text = string.Empty;
                    CodigoTextBox.PasswordChar = (char)0;
                    CodigoTextBox.Focus();
                }
            }
            else
                await IniciarVenta(true);
        }
        public Resultado AgregarImporteDeRetiroAsync()
        {
            Resultado<decimal> resultado = ValidarImporte();
            if (resultado.Error)
                return resultado;

            decimal importe = resultado.Datos;

            ProductosDataGridView.Rows.Add(_cantidad, "", "", $"{importe:0.00}", $"{importe * _cantidad:0.00}");
            ProductosDataGridView.Rows[^1].Selected = true;
            ProductosDataGridView.FirstDisplayedScrollingRowIndex = ProductosDataGridView.Rows.Count - 1;
            CodigoTextBox.Text = string.Empty;
            _cantidad = 1;
            ObtenerTotal();

            return resultado;
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
        private async Task ObtenerCambio()
        {
            decimal total = CalcularTotal();
            _pago += Convert.ToDecimal(CodigoTextBox.Text);
            decimal diferencia = _pago - total;
            if (diferencia < 0)
            {
                TotalLabel.Text = $"Resta {Math.Abs(diferencia):$0.00}";
                TotalLabel.ForeColor = Color.Gray;
            }
            else
            {
                await IniciarVenta(true);
                TotalLabel.Text = $"Cambio {diferencia:$0.00}";
                TotalLabel.ForeColor = Color.Black;
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

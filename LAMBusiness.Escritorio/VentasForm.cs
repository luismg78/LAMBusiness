using LAMBusiness.Backend;
using LAMBusiness.Contextos;
using LAMBusiness.Escritorio.Reportes;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.DTO.Movimiento;
using LAMBusiness.Shared.Movimiento;
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
            CorteDeCaja,
            ImprimirTicket
        }
        #endregion

        #region Variables globales del formulario
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
            _configuracion = configuracion;
            _cantidad = 1;
            UsuarioLabel.Text = $"{Global.PrimerApellido} {Global.SegundoApellido} {Global.Nombre} | {DateTime.Now:dd \\de MMMM \\de yyyy}";
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
                case Keys.F3:
                    ImprimirVentaModal();
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
        private async void ProductosDataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DataGridView row = (DataGridView)sender;
            int i = row.CurrentRow.Index;
            switch (_proceso)
            {
                case Proceso.Capturar:
                    e.Cancel = true;
                    await RemoverDetalleDeVenta(i);
                    break;
                case Proceso.Retirar:
                    e.Cancel = false;
                    break;
                default:
                    e.Cancel = true;
                    break;
            }

            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.PasswordChar = (char)0;
            CodigoTextBox.Focus();
        }

        private void ProductosDataGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            switch (_proceso)
            {
                case Proceso.Retirar:
                    ObtenerTotal();
                    Notificar("Importe eliminado");
                    break;
            }
        }

        private async Task RemoverDetalleDeVenta(int i)
        {
            decimal cantidad = Convert.ToDecimal(ProductosDataGridView.Rows[i].Cells[0].Value) * -1;
            decimal precio = Convert.ToDecimal(ProductosDataGridView.Rows[i].Cells[3].Value);
            Guid id = Guid.Parse(ProductosDataGridView.Rows[i].Cells[5].Value.ToString()!);

            string pregunta = "¿Desea eliminar el detalle del producto?";
            Color color = Color.Red;
            if (cantidad > 0)
            {
                color = Color.Black;
                pregunta = "¿Desea restablecer el detalle del producto?";
            }

            if (MessageBox.Show(pregunta, "Eliminación parcial", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                using var contexto = new DataContext(_configuracion);
                Ventas ventas = new(contexto);
                var resultado = await ventas.CancelarVentaParcial(id, (Guid)Global.UsuarioId!);
                if (!resultado.Error)
                {
                    var importe = precio * cantidad;
                    ProductosDataGridView.Rows[i].Cells[0].Value = $"{cantidad:0.0000}";
                    ProductosDataGridView.Rows[i].Cells[4].Value = $"{importe:0.00}";
                    ProductosDataGridView.Rows[i].DefaultCellStyle.ForeColor = color;
                    ObtenerTotal();
                    Notificar("El producto fue cancelado con éxito.");
                }
                else
                {
                    Notificar(resultado.Mensaje);
                }
            }
            else
            {
                Notificar("Operación cancelada por el usuario");
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
            IconoPictureBox.Location = new System.Drawing.Point(12, 25);
            IconoPictureBox.Size = new Size(80, 47);
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
                    IconoPictureBox.Image = Properties.Resources.cortedecaja;
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
        private void ImprimirTicketButton_Click(object sender, EventArgs e)
        {
            ImprimirVentaModal();
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
            using var contexto = new DataContext(_configuracion);
            Ventas ventas = new(contexto);
            Resultado<decimal> resultado = new();

            resultado = ValidarImporte();
            if (resultado.Error)
                return resultado;

            decimal importe = resultado.Datos;
            var venta = await ventas.Aplicar(_ventaId, _usuarioId, importe);
            if (venta.Error)
            {
                resultado.Error = true;
                resultado.Mensaje = venta.Mensaje;
                return resultado;
            }

            //ImprimirTicketDeVentaPorIdAsync(_ventaId);
            await ObtenerCambio();
            return resultado;
        }
        public async Task<Resultado> BuscarProductoPorCodigoAsync()
        {
            Notificar();
            Resultado resultado = new();
            BuscarForm form = new(_configuracion);
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
        public void ImprimirVentaModal()
        {
            Global.VentaId = null;
            var form = new ImprimirTicketForm(_configuracion);
            form.ShowDialog();
            if (Global.VentaId != null && Global.VentaId != Guid.Empty)
                ImprimirTicketDeVentaPorIdAsync((Guid)Global.VentaId);
            else
                Notificar("Impresión cancelada.");
            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.Focus();
        }
        public async Task<Resultado> ObtenerProductoPorCodigoAsync()
        {
            using var contexto = new DataContext(_configuracion);
            Ventas ventas = new(contexto);
            var resultado = await ventas.ObtenerProducto(_ventaId, _usuarioId, CodigoTextBox.Text, _cantidad);
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
            ProductosDataGridView.Rows.Add(producto.Cantidad, producto.Productos.Codigo, producto.Productos.Nombre, $"{producto.Productos.PrecioVenta:0.00}", $"{producto.Productos.PrecioVenta * producto.Cantidad:0.00}", producto.VentaNoAplicadaDetalleID);
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
            using var contexto = new DataContext(_configuracion);
            Ventas ventas = new(contexto);
            _ventaId = id;
            var resultado = await ventas.RecuperarVentaPorId(id, _usuarioId);
            if (!resultado.Error)
            {
                var resultadoDeVentas = resultado.Datos;
                if (resultadoDeVentas.VentasNoAplicadasDetalle != null && resultadoDeVentas.VentasNoAplicadasDetalle.Any())
                {
                    Color color = Color.Black;
                    int reg = 0;
                    foreach (var venta in resultadoDeVentas.VentasNoAplicadasDetalle)
                    {
                        color = Color.Black;
                        if (venta.Cantidad < 0)
                            color = Color.Red;
                        ProductosDataGridView.Rows.Add(venta.Cantidad, venta.Productos.Codigo, venta.Productos.Nombre, $"{venta.Productos.PrecioVenta:0.00}", $"{venta.Productos.PrecioVenta * venta.Cantidad:0.00}", venta.VentaNoAplicadaDetalleID);
                        ProductosDataGridView.Rows[^1].Selected = true;
                        ProductosDataGridView.FirstDisplayedScrollingRowIndex = ProductosDataGridView.Rows.Count - 1;
                        ProductosDataGridView.Rows[reg].DefaultCellStyle.ForeColor = color;
                        reg++;
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
            using var contexto = new DataContext(_configuracion);
            bool ok = HayRegistrosDeVentasPorAplicar();
            if (ok)
            {
                Notificar("Opción no aprobada, venta en proceso.");
            }
            else
            {
                var hayVentasNoAplicadas = await (from v in contexto.VentasNoAplicadas
                                                  join d in contexto.VentasNoAplicadasDetalle
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
            CodigoTextBox.CharacterCasing = CharacterCasing.Upper;
            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.PasswordChar = (char)0;
            CodigoTextBox.Focus();
        }
        private async Task IniciarVenta(bool nuevaVenta = false)
        {
            using var contexto = new DataContext(_configuracion);
            Ventas ventas = new(contexto);
            var resultado = await ventas.Inicializar((Guid)Global.UsuarioId!, nuevaVenta);
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
                CodigoTextBox.CharacterCasing = CharacterCasing.Upper;
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
                CodigoTextBox.CharacterCasing = CharacterCasing.Upper;
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
                CodigoTextBox.CharacterCasing = CharacterCasing.Normal;
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
                CodigoTextBox.CharacterCasing = CharacterCasing.Upper;
                CodigoTextBox.Text = string.Empty;
                CodigoTextBox.PasswordChar = (char)0;
                CodigoTextBox.Focus();
            }
        }
        #endregion

        #region Corte de cajas
        public async Task<Resultado> AplicarCorteDeCaja()
        {
            using var contexto = new DataContext(_configuracion);
            Sesiones sesion = new(contexto);
            Ventas ventas = new(contexto);
            var pwd = Sesiones.GenerateSHA512String(CodigoTextBox.Text);
            Resultado resultado = await sesion.ValidarContraseñaPorUsuarioIdAsync(_usuarioId, pwd);
            if (resultado.Error)
                return resultado;

            var resultadoCorteDeCaja = await ventas.CerrarVentas(_usuarioId);
            if (resultadoCorteDeCaja.Error)
            {
                resultado.Error = true;
                resultado.Mensaje = resultadoCorteDeCaja.Mensaje;
                return resultado;
            }

            var corte = resultadoCorteDeCaja.Datos;

            ProductosDataGridView.Rows.Add(1, "", "Importe del sistema", "", $"{corte.ImporteDelSistema:0.00}", "");
            ProductosDataGridView.Rows.Add(1, "", "Importe del usuario (retiros)", "", $"{corte.ImporteDelUsuario:0.00}", "");
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

            ImprimirTicketDeCorteDeCajaAsync(corte);
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
                    using var contexto = new DataContext(_configuracion);
                    Ventas ventas = new(contexto);
                    decimal importe = CalcularTotal();
                    var retiro = await ventas.RetirarEfectivoDeCaja(importe, _usuarioId);
                    if (!retiro.Error)
                    {
                        ImprimirTicketDeRetiroDeCajaAsync(retiro.Datos);
                        await IniciarVenta(true);
                    }
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

            ProductosDataGridView.Rows.Add(_cantidad, "", "", $"{importe:0.00}", $"{importe * _cantidad:0.00}", "");
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

        #region Impresiones
        public void ImprimirTicketDeCorteDeCajaAsync(CorteDeCajaDTO corteDeCaja)
        {
            TicketCorteDeCajaReport rpt = new();

            List<TicketDTO> ticket = new();
            TicketDTO ticketDTO = CargarRazonSocialYSucursal(new TicketDTO());
            ticketDTO.AtendidoPor = corteDeCaja.Usuario.ToUpper();
            ticketDTO.Fecha = corteDeCaja.Fecha.ToString("dd \\de MMMM \\de yyyy HH:mm");
            ticketDTO.ImporteTotalDeSistema = corteDeCaja.ImporteDelSistema.ToString("$#,###,##0.00");
            ticketDTO.ImporteTotalDeRetiros = corteDeCaja.ImporteDelUsuario.ToString("$#,###,##0.00");
            var diferencia = corteDeCaja.ImporteDelSistema - corteDeCaja.ImporteDelUsuario;
            if (diferencia > 0)
                ticketDTO.ImporteTotalDeVenta = $"Faltante {diferencia:$0.00}";
            else if (diferencia < 0)
                ticketDTO.ImporteTotalDeVenta = $"Sobrante {Math.Abs(diferencia):$0.00}";
            else
                ticketDTO.ImporteTotalDeVenta = $"Correcto {diferencia:$0.00}";

            ticket.Add(ticketDTO);
            rpt.DataSource = ticket;
            rpt.CreateDocument();
            try
            {
                //string ruta = AppDomain.CurrentDomain.BaseDirectory;
                //var pdf = Path.Combine(ruta, "Reportes", "ticketDeCorteDeCaja.pdf");
                //rpt.ExportToPdf(pdf);
                rpt.Print();
            }
            catch (Exception)
            {
                Notificar("Error de impresión. Verifique que la impresora esté encendida o conectada a la PC.");
            }
        }
        public void ImprimirTicketDeRetiroDeCajaAsync(RetiroCaja retiro)
        {
            TicketRetiroReport rpt = new();

            List<TicketDTO> ticket = new();
            TicketDTO ticketDTO = CargarRazonSocialYSucursal(new TicketDTO());
            ticketDTO.AtendidoPor = retiro.Usuarios.NombreCompleto.ToUpper();
            ticketDTO.Fecha = retiro.Fecha?.ToString("dd \\de MMMM \\de yyyy HH:mm");
            ticketDTO.ImporteTotalDeVenta = retiro.Importe.ToString("$#,###,##0.00");

            ticket.Add(ticketDTO);
            rpt.DataSource = ticket;
            rpt.CreateDocument();
            try
            {
                //string ruta = AppDomain.CurrentDomain.BaseDirectory;
                //var pdf = Path.Combine(ruta, "Reportes", "ticketDeRetiroDeCaja.pdf");
                //rpt.ExportToPdf(pdf);
                rpt.Print();
            }
            catch (Exception)
            {
                Notificar("Error de impresión. Verifique que la impresora esté encendida o conectada a la PC.");
            }
        }
        public void ImprimirTicketDeVentaAsync(TicketDTO venta)
        {
            TicketReport rpt = new();
            List<TicketDTO> ticket = new();
            //obtener datos de la sucursal y razón social.
            venta = CargarRazonSocialYSucursal(venta);

            ticket.Add(venta);
            rpt.DataSource = ticket;
            rpt.CreateDocument();
            try
            {
                //string ruta = AppDomain.CurrentDomain.BaseDirectory;
                //var pdf = Path.Combine(ruta, "Reportes", "ticketDeVenta.pdf");
                //rpt.ExportToPdf(pdf);
                rpt.Print();
            }
            catch (Exception)
            {
                Notificar("Error de impresión. Verifique que la impresora esté encendida o conectada a la PC.");
            }
        }
        public async void ImprimirTicketDeVentaPorIdAsync(Guid ventaId)
        {
            using var contexto = new DataContext(_configuracion);
            Ventas ventas = new(contexto);
            var resultado = await ventas.ObtenerVentaAsync(ventaId);
            if (!resultado.Error)
            {

                var venta = resultado.Datos;
                TicketDTO ticket = new()
                {
                    AtendidoPor = venta.Usuarios.NombreCompleto.ToUpper(),
                    Fecha = venta.Fecha?.ToString("dd/MM/yyyy HH:mm"),
                    Folio = venta.Folio.ToString("000000000"),
                    ImporteTotalDeVenta = venta.ImporteTotal.ToString("$#,###,###,##0.00"),
                    DetalleDeVenta = new()
                };

                if (venta.VentasDetalle != null && venta.VentasDetalle.Count > 0)
                {
                    List<TicketDetalleDTO> detalle = new();
                    foreach (var item in venta.VentasDetalle)
                    {
                        detalle.Add(new()
                        {
                            Cantidad = $"{item.Cantidad:0.0000}",
                            NombreDelProducto = item.Productos.Nombre.ToUpper(),
                            Precio = $"{item.PrecioVenta:$0.00}",
                            Importe = $"{item.Cantidad * item.PrecioVenta:$0.00}",
                        });
                    }

                    ticket.DetalleDeVenta.AddRange(detalle);
                }
                ImprimirTicketDeVentaAsync(ticket);
            }

            Notificar(resultado.Mensaje);
        }
        private TicketDTO CargarRazonSocialYSucursal(TicketDTO ticket)
        {
            var almacen = Global.Almacen;
            var rs = Global.RazonSocial;
            ticket.Sucursal = almacen.Nombre.ToUpper();
            ticket.Colonia = rs.Colonia.ToUpper();
            ticket.Domicilio = $"{rs.Domicilio.ToUpper()} C.P.{rs.CodigoPostal:00000}";
            ticket.Lugar = rs.Lugar.ToUpper();
            ticket.Nombre = rs.Nombre.ToUpper();
            ticket.NombreCorto = rs.NombreCorto.ToUpper();
            ticket.RFC = rs.RFC == null ? "" : rs.RFC.ToUpper();
            ticket.Telefono = rs.Telefono == null ? "" : $"TELÉFONO {rs.Telefono}";
            ticket.Slogan = rs.Slogan == null ? "" : rs.Slogan.ToUpper();

            return ticket;
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

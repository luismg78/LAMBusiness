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
        private readonly DataContext _contexto;
        private readonly Configuracion _configuracion;
        private decimal _cantidad;
        private decimal _pago;
        private Proceso _proceso = Proceso.Capturar;
        #endregion

        #region Constructor
        public VentasForm(Configuracion configuracion)
        {
            InitializeComponent();
            DataContext contexto = new(configuracion);
            //_ventas = new(contexto);
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
                IniciarSesionForm form = new(_contexto, _configuracion);
                Hide();
                form.Show();
                IniciarVenta();
            }
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
                            resultado = await ObtenerProductoAsync();
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
                    //var ayuda = new VentaAyudaForm();
                    //ayuda.ShowDialog();
                    break;
                case Keys.F5:
                    IniciarCobro();
                    break;
                case Keys.F7:
                    MessageBox.Show("F7");
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

        #region Ventas
        private async Task<Resultado> AplicarVentaAsync()
        {
            Resultado resultado = new();
            resultado = ValidarCantidad();
            if (!resultado.Error)
                ObtenerCambio();

            return resultado;
        }
        private async Task<Resultado> ObtenerProductoAsync()
        {
            Resultado resultado = new();

            //var resultadoProducto = await _productos.ObtenerRegistroPorCodigoAsync(CodigoTextBox.Text.Trim(), _razonSocialId);
            var producto = await _productos.ObtenerRegistroPorCodigoAsync(CodigoTextBox.Text.Trim());
            if (producto == null)
            {
                resultado.Error = true;
                resultado.Mensaje = "El código del producto es incorrecto.";
                return resultado;
            }

            ProductosDataGridView.Rows.Add(_cantidad, producto.Codigo, producto.Nombre, producto.PrecioVenta, producto.PrecioVenta * _cantidad);
            ProductosDataGridView.Rows[^1].Selected = true;
            ProductosDataGridView.FirstDisplayedScrollingRowIndex = ProductosDataGridView.Rows.Count - 1;

            CodigoTextBox.Text = string.Empty;
            _cantidad = 1;
            ObtenerTotal();

            return resultado;
        }
        #endregion

        #region Reseteo
        private void IniciarVenta()
        {
            _cantidad = 1;
            _pago = 0;
            _proceso = Proceso.Capturar;
            ProductosDataGridView.Rows.Clear();
            VentaTotalLabel.Text = "$0.00";
            CodigoLabel.Text = "Código";
            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.Focus();
        }
        private void IniciarCaptura()
        {
            _cantidad = 1;
            _proceso = Proceso.Capturar;
            CodigoLabel.Text = "Código";
            CodigoTextBox.Text = string.Empty;
            CodigoTextBox.Focus();
        }
        private void IniciarCobro()
        {
            _pago = 0;
            _proceso = Proceso.Aplicar;
            ObtenerTotal();
            CodigoLabel.Text = "Importe";
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
        #endregion
     
    }
}

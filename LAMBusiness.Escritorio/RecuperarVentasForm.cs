using LAMBusiness.Backend;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using System.Data;

namespace LAMBusiness.Escritorio
{
    public partial class RecuperarVentasForm : Form
    {
        private readonly Configuracion _configuracion;
        //private readonly Ventas _ventas;
        private Guid _ventaId;

        public RecuperarVentasForm(Configuracion configuracion, Guid ventaId)
        {
            InitializeComponent();
            //DataContext contexto = new(configuracion);
            _configuracion = configuracion;
            //_ventas = new Ventas(contexto);
            _ventaId = ventaId;
        }

        private async void RecuperarVentasForm_Load(object sender, EventArgs e)
        {
            using var contexto = new DataContext(_configuracion);
            Ventas ventas = new(contexto);
            var filtro = await ventas.RecuperarVenta((Guid)Global.UsuarioId!);
            if (filtro.Error)
            {
                _ventaId = Guid.Empty;
                Close();
            }

            if (filtro.Datos != null)
            {
                var resultadoDeVentas = filtro.Datos;
                if (resultadoDeVentas != null && resultadoDeVentas.Datos.Any())
                {
                    foreach (var venta in resultadoDeVentas.Datos.OrderByDescending(v => v.Fecha))
                    {
                        if (venta.Fecha != null)
                        {
                            var fecha = (DateTime)venta.Fecha;
                            RecuperarVentasDataGridView.Rows.Add(venta.VentaNoAplicadaID, fecha.ToString("ddd, dd \\de MMM \\de yyyy HH:mm"));
                            RecuperarVentasDataGridView.Rows[^1].Selected = true;
                        }
                    }
                }
            }
        }

        private void RecuperarVentasDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            RecuperarDetalleDeVenta(e.RowIndex);
        }

        private void RecuperarVentasDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataGridView row = (DataGridView)sender;
                RecuperarDetalleDeVenta(row.CurrentRow.Index);
            }
        }

        private void RecuperarDetalleDeVenta(int index)
        {
            _ventaId = (Guid)RecuperarVentasDataGridView.Rows[index].Cells[0].Value;
            Close();
        }

        private void RecuperarVentasForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.VentaId = _ventaId;
        }
    }
}

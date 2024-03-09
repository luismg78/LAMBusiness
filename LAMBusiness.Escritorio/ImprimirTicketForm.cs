using DocumentFormat.OpenXml.Office2010.Excel;
using LAMBusiness.Backend;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.DTO.Movimiento;

namespace LAMBusiness.Escritorio
{
    public partial class ImprimirTicketForm : Form
    {
        private readonly Configuracion _configuracion;
        public List<VentasDTO>? ventas;

        public ImprimirTicketForm(Configuracion configuracion)
        {
            InitializeComponent();
            _configuracion = configuracion;
        }

        private void ImprimirTicket_Load(object sender, EventArgs e)
        {
            CargarListaDeVentas();
        }

        private async void CargarListaDeVentas()
        {
            if (TicketDataGridView.Rows.Count > 0)
                TicketDataGridView.Rows.Clear();

            using var contexto = new DataContext(_configuracion);
            Ventas venta = new(contexto);

            var filtro = new Filtro<List<VentasDTO>>()
            {
                Skip = 0
            };

            var resultado = await venta.ObtenerVentasAsync(filtro);
            ventas = resultado.Datos;

            if (ventas != null && ventas.Count > 0)
            {
                string folio, importe;
                foreach (var item in ventas)
                {
                    folio = item.Folio.ToString("0000000");
                    importe = item.ImporteTotal.ToString("$0.00");
                    TicketDataGridView.Rows.Add(item.VentaID, folio, item.Usuarios.NombreCompleto.ToUpper(), item.Fecha, importe);
                }
                TicketDataGridView.Rows[0].Selected = true;
                TicketDataGridView.FirstDisplayedScrollingRowIndex = 0;
            }
        }

        private void TicketDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Global.VentaId = null;
                    Close();
                    break;
                case Keys.Enter:
                    if (TicketDataGridView != null && TicketDataGridView.Rows.Count > 0)
                    {
                        DataGridView row = (DataGridView)sender;
                        int index = row.CurrentRow.Index;
                        var ventaId = TicketDataGridView.Rows[index].Cells[0].Value.ToString()!;
                        Imprimir(ventaId);
                    }
                    break;
            }
        }

        private void Imprimir(string id)
        {
            Global.VentaId = new Guid(id);
            Close();
        }

        private void TicketDataGridView_DoubleClick(object sender, EventArgs e)
        {
            if (TicketDataGridView != null && TicketDataGridView.Rows.Count > 0)
            {
                DataGridView row = (DataGridView)sender;
                int index = row.CurrentRow.Index;
                var ventaId = TicketDataGridView.Rows[index].Cells[0].Value.ToString()!;
                Imprimir(ventaId);
            }
        }
    }
}

using LAMBusiness.Backend;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;

namespace LAMBusiness.Escritorio
{
    public partial class FormasDePagoForm : Form
    {
        private readonly Configuracion _configuracion;
        private byte _formaDePagoId;
        private string _formaDePagoNombre;

        public FormasDePagoForm(Configuracion configuracion)
        {
            InitializeComponent();
            _configuracion = configuracion;
            _formaDePagoId = 0;
            _formaDePagoNombre = "";
        }

        private async void FormasDePagoForm_Load(object sender, EventArgs e)
        {
            using var contexto = new DataContext(_configuracion);
            FormasDePagos formasDePago = new(contexto);
            var filtro = await formasDePago.ObtenerRegistrosAsync();
            if (filtro.Error)
                Close();

            if (filtro.Datos != null)
            {
                var resultadoDeFormasDePago = filtro.Datos;
                if (resultadoDeFormasDePago != null && resultadoDeFormasDePago.Any())
                {
                    foreach (var formaDePago in resultadoDeFormasDePago.OrderByDescending(v => v.Nombre))
                    {
                        if (formaDePago.FormaPagoID > 0)
                        {
                            FormasDePagoDataGridView.Rows.Add(formaDePago.FormaPagoID, formaDePago.Nombre);
                            FormasDePagoDataGridView.Rows[^1].Selected = true;
                        }
                    }
                }
            }
        }

        private void FormasDePagoDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ObtenerFormaDePago(e.RowIndex);
        }

        private void FormasDePagoDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataGridView row = (DataGridView)sender;
                ObtenerFormaDePago(row.CurrentRow.Index);
            }
        }

        private void ObtenerFormaDePago(int index)
        {
            _formaDePagoId = (byte)FormasDePagoDataGridView.Rows[index].Cells[0].Value;
            _formaDePagoNombre = (string)FormasDePagoDataGridView.Rows[index].Cells[1].Value;
            Close();
        }

        private void FormasDePagoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.FormaDePago = new Shared.Catalogo.FormaPago
            {
                FormaPagoID = _formaDePagoId,
                Nombre = _formaDePagoNombre
            };
        }
    }
}

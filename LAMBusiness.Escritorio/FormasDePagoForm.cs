using LAMBusiness.Backend;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.Catalogo;

namespace LAMBusiness.Escritorio
{
    public partial class FormasDePagoForm : Form
    {
        private readonly Configuracion _configuracion;
        private FormaPago _formaDePago;
        //private byte _formaDePagoId;
        //private string _formaDePagoNombre;

        public FormasDePagoForm(Configuracion configuracion)
        {
            InitializeComponent();
            _configuracion = configuracion;
            _formaDePago = new();
            //_formaDePagoId = 0;
            //_formaDePagoNombre = "";
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
                            string porcentaje = formaDePago.PorcentajeDeCobroExtra > 0 ? $"{formaDePago.PorcentajeDeCobroExtra}%" : "0";
                            FormasDePagoDataGridView.Rows.Add(formaDePago.FormaPagoID, formaDePago.Nombre,porcentaje);
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

        private async void ObtenerFormaDePago(int index)
        {
            using var contexto = new DataContext(_configuracion);
            FormasDePagos formasDePago = new(contexto);
            byte formaDePagoId = (byte)FormasDePagoDataGridView.Rows[index].Cells[0].Value;
            var formaDePagoResult = await formasDePago.ObtenerRegistroAsync(formaDePagoId);
            if (!formaDePagoResult.Error)
                _formaDePago = formaDePagoResult.Datos;
            else
                _formaDePago = new();
            Close();
        }

        private void FormasDePagoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.FormaDePago = _formaDePago;
        }
    }
}

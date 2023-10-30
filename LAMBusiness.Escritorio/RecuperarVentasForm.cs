using DocumentFormat.OpenXml.Office2010.Excel;
using LAMBusiness.Backend;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.Movimiento;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAMBusiness.Escritorio
{
    public partial class RecuperarVentasForm : Form
    {
        private readonly Configuracion _configuracion;
        private readonly Ventas _ventas;
        private Guid _ventaId;

        public RecuperarVentasForm(Configuracion configuracion, Guid ventaId)
        {
            InitializeComponent();
            DataContext contexto = new(configuracion);
            _configuracion = configuracion;
            _ventas = new Ventas(contexto);
            _ventaId = ventaId;
        }

        private async void RecuperarVentasForm_Load(object sender, EventArgs e)
        {
            var filtro = await _ventas.RecuperarVenta((Guid)Global.UsuarioId!);
            if (filtro.Error)
            {
                MensajeDeError(filtro.Mensaje);
                Close();
            }

            if (filtro.Datos != null)
            {
                var ventas = filtro.Datos;
                if (ventas != null && ventas.Datos.Any())
                {
                    foreach (var venta in ventas.Datos.OrderByDescending(v => v.Fecha))
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

        private static void MensajeDeError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

﻿using LAMBusiness.Backend;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.Catalogo;

namespace LAMBusiness.Escritorio
{
    public partial class BuscarForm : Form
    {
        private readonly Configuracion _configuracion;
        public string Codigo;
        public List<Producto>? productos;

        public BuscarForm(Configuracion configuracion)
        {
            InitializeComponent();
            _configuracion = configuracion;
            Codigo = string.Empty;
        }

        private void BuscarForm_Load(object sender, EventArgs e)
        {
            Notificar();
            CargarListaDeProductos();
        }

        private async void CargarProductos(string? patron)
        {
            productos = await ObtenerProductos(patron);
            if (ProductosDataGridView != null)
                CargarListaDeProductos();
        }

        private void CargarListaDeProductos()
        {
            if (ProductosDataGridView.Rows.Count > 0)
                ProductosDataGridView.Rows.Clear();

            if (productos != null && productos.Any())
            {
                foreach (var producto in productos)
                {
                    ProductosDataGridView.Rows.Add(producto.Codigo, producto.Nombre, producto.PrecioVenta);
                }
                ProductosDataGridView.Rows[0].Selected = true;
                ProductosDataGridView.FirstDisplayedScrollingRowIndex = 0;
            }
        }

        private void CerrarButton_Click(object sender, EventArgs e)
        {
            Codigo = string.Empty;
            Close();
        }

        public async Task<List<Producto>> ObtenerProductos(string? patron)
        {
            if (!string.IsNullOrEmpty(patron))
            {
                using var contexto = new DataContext(_configuracion);
                Productos producto = new(contexto);
                ProductoTextBox.Text = patron.Trim();
                var filtro = new Filtro<List<Producto>>()
                {
                    Patron = ProductoTextBox.Text,
                    Skip = 0,
                };
                filtro = await producto.ObtenerRegistros(filtro);
                if (filtro != null)
                    return filtro.Datos;
            }

            return new List<Producto>();
        }

        private void Notificar(string mensaje = "")
        {
            //NotificacionLabel.Text = mensaje;
            NotificacionBuscarToolStripStatus.Text = mensaje;
        }

        private void ProductoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    ProductoTextBox.Text = string.Empty;
                    Codigo = string.Empty;
                    Close();
                    break;
                case Keys.Enter:
                    if (ProductoTextBox.Text.Length == 0)
                        ProductosDataGridView.Rows.Clear();
                    else if (ProductoTextBox.Text.Length > 2)
                    {
                        CargarProductos(ProductoTextBox.Text);
                        if (ProductosDataGridView != null && ProductosDataGridView.Rows.Count > 0)
                            ProductosDataGridView.Focus();
                    }
                    else
                    {
                        Notificar("Teclee al menos tres caracteres...");
                    }
                    break;
            }
        }

        private void ProductosDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    ProductoTextBox.Text = string.Empty;
                    Codigo = string.Empty;
                    Close();
                    break;
                case Keys.Enter:
                    Codigo = "";
                    if (ProductosDataGridView != null && ProductosDataGridView.Rows.Count > 0)
                    {
                        DataGridView row = (DataGridView)sender;
                        int index = row.CurrentRow.Index;
                        Codigo = ProductosDataGridView.Rows[index].Cells[0].Value.ToString()!;
                    }
                    Close();
                    break;
            }
        }

        private void ProductosDataGridView_DoubleClick(object sender, EventArgs e)
        {
            Codigo = "";
            if (ProductosDataGridView != null && ProductosDataGridView.Rows.Count > 0)
            {
                DataGridView row = (DataGridView)sender;
                int index = row.CurrentRow.Index;
                Codigo = ProductosDataGridView.Rows[index].Cells[0].Value.ToString()!;
            }
            Close();
        }
    }
}

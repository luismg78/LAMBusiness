using LAMBusiness.Shared.Catalogo;

namespace LAMBusiness.Escritorio
{
    public partial class BuscarForm : Form
    {
        public Producto producto;

        public BuscarForm()
        {
            InitializeComponent();
        }

        private void BuscarForm_Load(object sender, EventArgs e)
        {
            decimal precioVenta = producto.PrecioVenta ?? 0;
            NombreDelProductoLabel.Text = producto.Nombre;
            PrecioVentaLabel.Text = precioVenta.ToString("$###,###,##0.00");
            ExistenciaLabel.Text = "0";
        }
    }
}

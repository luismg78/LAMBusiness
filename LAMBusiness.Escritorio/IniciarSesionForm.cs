using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using System.Text.RegularExpressions;

namespace LAMBusiness.Escritorio
{
    public partial class IniciarSesionForm : Form
    {
        ErrorProvider _error;
        private readonly DataContext _contexto;
        private readonly Configuracion _configuracion;

        public IniciarSesionForm(DataContext contexto,Configuracion configuracion)
        {
            InitializeComponent();
            _error = new ErrorProvider();
            _contexto = contexto;
            _configuracion = configuracion;
        }

        private void IniciarSesionButton_Click(object sender, EventArgs e)
        {
            var ok = ValidarCampos(CorreoElectronicoTextBox.Text, PasswordTextBox.Text);
            if (ok)
            {
                Global.UsuarioId = Guid.NewGuid();
                Global.AplicacionCerrada = false;
                VentasForm form = new(_configuracion);
                form.Show();
                this.Close();
            }
            VentasForm frm = new(_configuracion);
            frm.Show();
        }

        private bool ValidarCampos(string correoElectronico, string password)
        {
            bool valid = true;
            _error.Clear();
            if (string.IsNullOrEmpty(correoElectronico))
            {
                _error.SetError(CorreoElectronicoTextBox, "valor requerido");
                valid = false;
            }
            if (string.IsNullOrEmpty(password))
            {
                _error.SetError(PasswordTextBox, "valor requerido");
                valid = false;
            }
            if (valid)
            {
                if (!Regex.IsMatch(correoElectronico, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                {
                    _error.SetError(CorreoElectronicoTextBox, "formato incorrecto");
                    valid = false;
                }
            }
            return valid;
        }

        private void SalirButton_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show("¿Desea cerrar la aplicación?", "Cerrar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (resultado == DialogResult.OK)
            {
                Global.AplicacionCerrada = true;
                Application.Exit();
            }
        }
    }
}
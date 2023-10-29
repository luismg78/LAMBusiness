using LAMBusiness.Backend;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.DTO.Sesion;
using System.Text.RegularExpressions;

namespace LAMBusiness.Escritorio
{
    public partial class IniciarSesionForm : Form
    {
        ErrorProvider _error;
        private readonly DataContext _contexto;
        private readonly Configuracion _configuracion;
        private readonly Sesiones _sesion;

        public IniciarSesionForm(Configuracion configuracion)
        {
            InitializeComponent();
            DataContext contexto = new(configuracion);
            _error = new ErrorProvider();
            _contexto = contexto;
            _configuracion = configuracion;
            _sesion = new Sesiones(contexto);
        }

        private void IniciarSesionForm_Load(object sender, EventArgs e)
        {
            NotificacionLabel.Text = string.Empty;
        }

        private async void IniciarSesionButton_ClickAsync(object sender, EventArgs e)
        {
            NotificacionLabel.Text = "Procesando inicio de sesión...";
            var ok = ValidarCampos(CorreoElectronicoTextBox.Text, PasswordTextBox.Text);
            if (ok)
            {
                InicioDeSesionDTO inicioDeSesion = new()
                {
                    Email = CorreoElectronicoTextBox.Text,
                    Password = Sesiones.GenerateSHA512String(PasswordTextBox.Text)
                };
                var usuario = await _sesion.IniciarSesion(inicioDeSesion);
                if (!usuario.Error)
                {
                    Global.UsuarioId = usuario.Datos.UsuarioID;
                    Global.Nombre = usuario.Datos.Nombre;
                    Global.PrimerApellido = usuario.Datos.PrimerApellido;
                    Global.SegundoApellido = usuario.Datos.SegundoApellido;
                    Global.AplicacionCerrada = false;
                    VentasForm form = new(_configuracion);
                    Hide();
                    form.Show();
                }
                NotificacionLabel.Text = usuario.Mensaje;
            }
        }

        private bool ValidarCampos(string correoElectronico, string password)
        {
            bool valid = true;
            _error.Clear();
            NotificacionLabel.Text = "Error de validación";
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

            NotificacionLabel.Text = string.Empty;
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
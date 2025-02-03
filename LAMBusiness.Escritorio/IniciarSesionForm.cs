using LAMBusiness.Backend;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.Catalogo;
using LAMBusiness.Shared.DTO.Sesion;
using System.Text.RegularExpressions;

namespace LAMBusiness.Escritorio
{
	public partial class IniciarSesionForm : Form
	{
		ErrorProvider _error;
		private readonly Configuracion _configuracion;

		public IniciarSesionForm(Configuracion configuracion)
		{
			InitializeComponent();
			_error = new ErrorProvider();
			_configuracion = configuracion;
		}

		private void IniciarSesionForm_Load(object sender, EventArgs e)
		{
			DateTime Fecha = DateTime.Now;
			DateTime FechaInicio = new(2025, 1, 1, 0, 0, 0);
			DateTime FechaFin = new(2025, 12, 31, 23, 59, 59);
			if (Fecha < FechaInicio || Fecha > FechaFin)
			{
				var resultado = MessageBox.Show("Lo sentimos, el periodo de vigencia ha expirado, comuníquese con el administrador del sistema.", "LAMBusinness", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Global.AplicacionCerrada = true;
				Application.Exit();
			}

			IniciarSesionButton.Enabled = true;
			NotificacionLabel.Text = string.Empty;
		}

		private void IniciarSesionButton_ClickAsync(object sender, EventArgs e)
		{
			IniciarSesion();
		}

		private async void IniciarSesion()
		{
			NotificacionLabel.Text = "Procesando inicio de sesión...";
			IniciarSesionButton.Enabled = false;
			var ok = ValidarCampos(CorreoElectronicoTextBox.Text, PasswordTextBox.Text);
			if (ok)
			{
				InicioDeSesionDTO inicioDeSesion = new()
				{
					Email = CorreoElectronicoTextBox.Text,
					Password = Sesiones.GenerateSHA512String(PasswordTextBox.Text)
				};

				DataContext contexto = new(_configuracion);
				Sesiones _sesion = new(contexto);

				var usuario = await _sesion.IniciarSesion(inicioDeSesion);
				if (!usuario.Error)
				{

					Global.UsuarioId = usuario.Datos.UsuarioID;
					Global.Nombre = usuario.Datos.Nombre;
					Global.PrimerApellido = usuario.Datos.PrimerApellido;
					Global.SegundoApellido = usuario.Datos.SegundoApellido;
					Global.AplicacionCerrada = false;
					Global.Almacen = await ObtenerAlmacen();
					Global.RazonSocial = await ObtenerRazonSocial();
					VentasForm form = new(_configuracion);
					Hide();
					form.Show();
				}
				NotificacionLabel.Text = usuario.Mensaje;
				IniciarSesionButton.Enabled = true;
			}
			else
			{
				IniciarSesionButton.Enabled = true;
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

		private void CorreoElectronicoTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
				IniciarSesion();
		}

		private void PasswordTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
				IniciarSesion();
		}

		private async Task<Almacen> ObtenerAlmacen()
		{
			DataContext contexto = new(_configuracion);
			Almacenes almacenes = new(contexto);

			if (_configuracion.AlmacenId != Guid.Empty)
			{
				var almacen = await almacenes.ObtenerRegistroPorIdAsync(_configuracion.AlmacenId);
				if (almacen != null)
					return almacen;
			}

			return new Almacen()
			{
				AlmacenID = Guid.Empty,
				Nombre = "Almacén no configurado"
			};
		}

		private async Task<RazonSocial> ObtenerRazonSocial()
		{
			DataContext contexto = new(_configuracion);
			RazonesSociales razonesSociales = new(contexto);

			if (_configuracion.RazonSocialId != Guid.Empty)
			{
				var razonSocial = await razonesSociales.ObtenerRegistroPorIdAsync(_configuracion.RazonSocialId);
				if (razonSocial != null)
					return razonSocial;
			}

			return new RazonSocial()
			{
				RazonSocialId = Guid.Empty,
				CodigoPostal = 0,
				Colonia = "No configurada",
				Domicilio = "No configurado",
				Lugar = "No configurado",
				Nombre = "No configurado",
				NombreCorto = "No configurado",
				RFC = "",
				Slogan = ""
			};
		}
	}
}
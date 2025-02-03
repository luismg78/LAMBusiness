namespace LAMBusiness.Web.Controllers
{
	using LAMBusiness.Backend;
	using LAMBusiness.Shared.Aplicacion;
	using LAMBusiness.Shared.Kardex;
	using LAMBusiness.Web.Helpers;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;
	using System;
	using System.Threading.Tasks;

	public class KardexController : GlobalController
	{
		private readonly ICombosHelper _combosHelper;
		private readonly IGetHelper _getHelper;
		private readonly IConfiguration _configuration;
		//private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly KardexDeMovimientos _kardex;
		private Guid moduloId = Guid.Parse("3B55740F-0263-410B-8A5C-65AFE195625C");

		public KardexController(Configuracion configuracion, ICombosHelper combosHelper, IGetHelper getHelper, IConfiguration configuration)
		{
			_combosHelper = combosHelper;
			_getHelper = getHelper;
			_configuration = configuration;
			_kardex = new KardexDeMovimientos(configuracion);
		}

		public async Task<IActionResult> Productos()
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "kardex");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
			{
				return RedirectToAction("Index", "Home");
			}

			var filtro = new Filtro<KardexDeProducto>()
			{
				Patron = "",
				PermisoEscritura = permisosModulo.PermisoEscritura,
				PermisoImprimir = permisosModulo.PermisoImprimir,
				PermisoLectura = permisosModulo.PermisoLectura,
				Skip = 0,
				Datos = new KardexDeProducto
				{
					AlmacenID = Guid.Empty,
					ProductoID = Guid.Empty,
					AlmacenesDDL = await _combosHelper.GetComboAlmacenesAsync(),
				}
			};

			return View(filtro.Datos);
		}

		public async Task<IActionResult> ProductosLista(Guid productoId, Guid almacenId)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "kardex");
			if (validateToken != null) { return null!; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
				return null!;

			var filtro = new Filtro<KardexDeProducto>()
			{
				Patron = "",
				PermisoEscritura = permisosModulo.PermisoEscritura,
				PermisoImprimir = permisosModulo.PermisoImprimir,
				PermisoLectura = permisosModulo.PermisoLectura,
				Skip = 0,
				Datos = new KardexDeProducto
				{
					AlmacenID = almacenId,
					ProductoID = productoId,
					AlmacenesDDL = await _combosHelper.GetComboAlmacenesAsync(),
					KardexDeProductoDetalle = await _kardex.KardexDeProductos(productoId, almacenId),
				}
			};

			return PartialView(filtro.Datos);
		}
	}
}

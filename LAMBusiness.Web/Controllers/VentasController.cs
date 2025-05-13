namespace LAMBusiness.Web.Controllers
{
	using Helpers;
	using Interfaces;
	using LAMBusiness.Backend;
	using LAMBusiness.Contextos;
	using LAMBusiness.Shared.DTO.Movimiento;
	using LAMBusiness.Shared.Movimiento;
	using LAMBusiness.Web.Models.ViewModels;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.ViewFeatures;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Shared.Aplicacion;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class VentasController : GlobalController
	{
		private readonly DataContext _context;
		private readonly IGetHelper _getHelper;
		private readonly IConverterHelper _converterHelper;
		private readonly IConfiguration _configuration;
		private readonly Configuracion _configuracion;
		private readonly IDashboard _dashboard;
		private readonly Productos _productos;
		private readonly Ventas _ventas;
		private Guid moduloId = Guid.Parse("a0ca4d51-b518-4a65-b1e3-f0a03b1caff8");

		public VentasController(DataContext context, IGetHelper getHelper,
			IConverterHelper converterHelper,
			IConfiguration configuration,
			Configuracion configuracion,
			IDashboard dashboard)
		{
			_context = context;
			_getHelper = getHelper;
			_converterHelper = converterHelper;
			_configuration = configuration;
			_dashboard = dashboard;
			_productos = new Productos(context);
			_ventas = new Ventas(context);
		}

		public async Task<IActionResult> Index()
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
			{
				return RedirectToAction("Index", "Home");
			}

			var f = DateTime.Now;
			var fechaInicio = new DateTime(f.Year, f.Month, f.Day, 0, 0, 0);
			var fechaFin = new DateTime(f.Year, f.Month, f.Day, 23, 59, 59);

			var retiros = (from v in _context.Ventas
						   join u in _context.Usuarios on v.UsuarioID equals u.UsuarioID
						   where u.AdministradorID != "SA" 
						      && v.Fecha >= fechaInicio && v.Fecha <= fechaFin 
								 //&& (v.VentaCierreID == Guid.Empty || v.VentaCierreID == null)
						   select new VentasViewModel()
						   {
							   VentaID = v.VentaID,
							   UsuarioID = u.UsuarioID,
							   Fecha = v.Fecha,
							   Folio = v.Folio,
							   Nombre = u.Nombre,
							   PrimerApellido = u.PrimerApellido,
							   SegundoApellido = u.SegundoApellido,
						   });

			var administrador = await EsAdministradorAsync();
			if (!administrador)
				retiros = retiros.Where(c => c.UsuarioID == token.UsuarioID);

			var filtro = new Filtro<List<VentasViewModel>>()
			{
				Datos = await retiros.OrderByDescending(q => q.Fecha)
				.ThenByDescending(u => u.PrimerApellido)
				.ThenByDescending(u => u.SegundoApellido)
				.ThenByDescending(u => u.Nombre)
				.Take(50).ToListAsync(),
				Patron = "",
				FechaInicio = fechaInicio,
				FechaFin = fechaFin,
				PermisoEscritura = permisosModulo.PermisoEscritura,
				PermisoImprimir = permisosModulo.PermisoImprimir,
				PermisoLectura = permisosModulo.PermisoLectura,
				Registros = await retiros.CountAsync(),
				Skip = 0
			};

			return View(filtro);

		}

		public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<VentasViewModel>> filtro, bool todos)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
			if (validateToken != null) { return null; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
				return null;

			var esAdministrador = await EsAdministradorAsync();

			IQueryable<VentasViewModel> query = (from r in _context.Ventas
												 join u in _context.Usuarios on r.UsuarioID equals u.UsuarioID
												 where u.AdministradorID != "SA"
												 select new VentasViewModel()
												 {
													 VentaID = r.VentaID,
													 UsuarioID = u.UsuarioID,
													 Fecha = r.Fecha,
													 Folio = r.Folio,
													 Nombre = u.Nombre,
													 PrimerApellido = u.PrimerApellido,
													 SegundoApellido = u.SegundoApellido,
												 });

			var fi = filtro.FechaInicio;
			var ff = filtro.FechaFin;
			var fechaInicio = new DateTime(fi.Year, fi.Month, fi.Day, 0, 0, 0);
			var fechaFin = new DateTime(ff.Year, ff.Month, ff.Day, 23, 59, 59);

			query = query.Where(r => r.Fecha >= fechaInicio && r.Fecha <= fechaFin);

			if (!esAdministrador)
				query = query.Where(c => c.UsuarioID == token.UsuarioID);


			if (filtro.Patron != null && filtro.Patron != "")
			{
				var words = filtro.Patron.Trim().ToUpper().Split(' ');
				foreach (var w in words)
				{
					if (w.Trim() != "")
					{
						query = query.Where(c => c.Nombre.Contains(w) ||
												 c.PrimerApellido.Contains(w) ||
												 c.SegundoApellido.Contains(w));
					}
				}
			}

			filtro.Registros = await query.CountAsync();

			filtro.Datos = await query.OrderByDescending(q => q.Fecha)
				.ThenByDescending(u => u.PrimerApellido)
				.ThenByDescending(u => u.SegundoApellido)
				.ThenByDescending(u => u.Nombre)
				.Skip(filtro.Skip)
				.Take(50)
				.ToListAsync();

			filtro.PermisoEscritura = permisosModulo.PermisoEscritura;
			filtro.PermisoImprimir = permisosModulo.PermisoImprimir;
			filtro.PermisoLectura = permisosModulo.PermisoLectura;

			return new PartialViewResult
			{
				ViewName = "_AddRowsNextAsync",
				ViewData = new ViewDataDictionary
							<Filtro<List<VentasViewModel>>>(ViewData, filtro)
			};
		}

		private async Task<bool> EsAdministradorAsync()
		{
			return await _context.Usuarios
				.Where(u => u.UsuarioID == token.UsuarioID && (u.AdministradorID == "SA" || u.AdministradorID == "GA"))
				.AnyAsync();
		}

		//public async Task<IActionResult> Index()
		//{
		//	var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
		//	if (validateToken != null) { return validateToken; }

		//	if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
		//	{
		//		return RedirectToAction("Index", "Home");
		//	}

		//	var ventas = _context.Ventas
		//		.Include(e => e.Usuarios)
		//		.OrderByDescending(e => e.Fecha);

		//	var filtro = new Filtro<List<Venta>>()
		//	{
		//		Datos = await ventas.Take(50).ToListAsync(),
		//		Patron = "",
		//		PermisoEscritura = permisosModulo.PermisoEscritura,
		//		PermisoImprimir = permisosModulo.PermisoImprimir,
		//		PermisoLectura = permisosModulo.PermisoLectura,
		//		Registros = await ventas.CountAsync(),
		//		Skip = 0
		//	};

		//	return View(filtro);

		//}

		//public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<Venta>> filtro)
		//{
		//	var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
		//	if (validateToken != null) { return null; }

		//	if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
		//	{
		//		return null;
		//	}

		//	IQueryable<Venta> query = null;
		//	if (filtro.Patron != null && filtro.Patron != "")
		//	{
		//		var words = filtro.Patron.Trim().ToUpper().Split(' ');
		//		foreach (var w in words)
		//		{
		//			if (w.Trim() != "")
		//			{
		//				if (query == null)
		//				{
		//					query = _context.Ventas
		//						.Include(e => e.Usuarios)
		//						.Where(p => p.Folio.ToString().Contains(w) ||
		//									p.Usuarios.Nombre.Contains(w) ||
		//									p.Usuarios.PrimerApellido.Contains(w) ||
		//									p.Usuarios.SegundoApellido.Contains(w));
		//				}
		//				else
		//				{
		//					query = query
		//						.Include(e => e.Usuarios)
		//						.Where(p => p.Folio.ToString().Contains(w) ||
		//									p.Usuarios.Nombre.Contains(w) ||
		//									p.Usuarios.PrimerApellido.Contains(w) ||
		//									p.Usuarios.SegundoApellido.Contains(w));
		//				}
		//			}
		//		}
		//	}
		//	if (query == null)
		//	{
		//		query = _context.Ventas.Include(e => e.Usuarios);
		//	}

		//	filtro.Registros = await query.CountAsync();

		//	filtro.Datos = await query.OrderByDescending(m => m.Fecha)
		//		.Skip(filtro.Skip)
		//		.Take(50)
		//		.ToListAsync();

		//	filtro.PermisoEscritura = permisosModulo.PermisoEscritura;
		//	filtro.PermisoImprimir = permisosModulo.PermisoImprimir;
		//	filtro.PermisoLectura = permisosModulo.PermisoLectura;

		//	return new PartialViewResult
		//	{
		//		ViewName = "_AddRowsNextAsync",
		//		ViewData = new ViewDataDictionary
		//					<Filtro<List<Venta>>>(ViewData, filtro)
		//	};
		//}

		public async Task<IActionResult> Details(Guid? id)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
			{
				return RedirectToAction(nameof(Index));
			}

			if (id == null)
			{
				TempData["toast"] = "Identificador incorrecto.";
				return RedirectToAction(nameof(Index));
			}

			var venta = await _getHelper.GetVentaByIdAsync((Guid)id);

			if (venta == null)
			{
				TempData["toast"] = "Identificador de la venta inexistente.";
				return RedirectToAction(nameof(Index));
			}

			var ventaViewModel = _converterHelper.ToVentaViewModel(venta);

			ventaViewModel.PermisoEscritura = permisosModulo.PermisoEscritura;

			return View(ventaViewModel);
		}

		//public async Task<IActionResult> Index()
		//{
		//	var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
		//	if (validateToken != null) { return validateToken; }

		//	if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
		//		return RedirectToAction(nameof(Index));

		//	var resultado = await _ventas.Inicializar(token.UsuarioID, true);
		//	if (resultado.Error)
		//	{
		//		TempData["toast"] = resultado.Mensaje;
		//		return RedirectToAction("Index", "Movimiento");
		//	}

		//	return View(resultado.Datos);
		//}

		//public async Task<IActionResult> Agregar(Guid? id)
		//{
		//	var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
		//	if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

		//	if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
		//	{
		//		return Json(new { Reiniciar = true, Error = true });
		//	}

		//	var agregar = await _ventas.Agregar(id, token.UsuarioID);
		//	return Json(new { Estatus = agregar.Mensaje, agregar.Reiniciar, agregar.Error });
		//}

		//public async Task<IActionResult> Aplicar(Guid? id)
		//{
		//var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
		//if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

		//if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
		//    return Json(new { Reiniciar = true, Error = true });

		//var venta = await _ventas.Aplicar(id, token.UsuarioID, formade);
		//if (venta.Error)
		//{
		//    await BitacoraAsync("Aplicar", venta.Datos, venta.Excepcion);
		//    return Json(new { Estatus = "Venta no realizada.", Error = true });
		//}

		//await BitacoraAsync("Aplicar", venta.Datos);
		//return PartialView(venta.Datos);
		//	return null;
		//}

		//public async Task<IActionResult> CerrarVentas()
		//{
		//	var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
		//	if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

		//	if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
		//		return Json(new { Reiniciar = true, Error = true });

		//	var corteDeCaja = await _ventas.CerrarVentas(token.UsuarioID);
		//	if (corteDeCaja.Error)
		//		return Json(new { Error = true, Estatus = corteDeCaja.Mensaje });

		//	return PartialView(corteDeCaja.Datos);
		//}

		//public async Task<IActionResult> RecuperarVentaPorId(Guid? id)
		//{
		//	var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
		//	if (validateToken != null) { return validateToken; }

		//	if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
		//		return RedirectToAction(nameof(Index));

		//	var venta = await _ventas.RecuperarVentaPorId(id, token.UsuarioID);
		//	if (venta.Error)
		//	{
		//		TempData["toast"] = venta.Mensaje;
		//		return RedirectToAction("Index", "Movimiento");
		//	}


		//	return View(nameof(Index), venta.Datos);
		//}

		//public async Task<IActionResult> RecuperarVenta()
		//{
		//	var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
		//	if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

		//	if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
		//	{
		//		return Json(new { Reiniciar = true, Error = true });
		//	}

		//	var filtro = await _ventas.RecuperarVenta(token.UsuarioID);
		//	if (filtro.Error)
		//		return Json(new { Estatus = filtro.Mensaje, Error = true });

		//	return PartialView(filtro.Datos);
		//}

		//public async Task<IActionResult> ObtenerProductoPorCodigo(Guid? id, string codigo, decimal cantidad)
		//{
		//	var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
		//	if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

		//	var resultado = await _ventas.ObtenerProducto(id, token.UsuarioID, codigo, cantidad);

		//	if (resultado.Error)
		//	{
		//		switch (resultado.Mensaje.Trim().ToLower())
		//		{
		//			case "buscarproducto":
		//				return Json(new { BuscarProducto = true, Error = true, Reiniciar = false });
		//			case "reiniciar":
		//				TempData["toast"] = "Identificador de la venta incorrecto.";
		//				return Json(new { Reiniciar = true, Error = true });
		//			default:
		//				return Json(new { Estatus = resultado.Mensaje, Error = true, Reiniciar = true });
		//		}
		//	}

		//	return PartialView(resultado.Datos);
		//}

		//public async Task<IActionResult> CancelarVenta(Guid? id)
		//{
		//	var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
		//	if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

		//	if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
		//	{
		//		return Json(new { Reiniciar = true, Error = true });
		//	}

		//	var ventaCancelada = await _ventas.CancelarVenta(id, token.UsuarioID);
		//	if (ventaCancelada.Error)
		//	{
		//		await BitacoraAsync("CancelarVenta", ventaCancelada.Datos, token.UsuarioID, ventaCancelada.Mensaje);
		//		return Json(new { Estatus = "Cancelación no realizada.", Error = true });
		//	}

		//	await BitacoraAsync("CancelarVenta", ventaCancelada.Datos, token.UsuarioID);
		//	return Json(new { Error = false });
		//}

		//public async Task<IActionResult> RetirarEfectivoDeCaja(decimal total)
		//{
		//	var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
		//	if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

		//	if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
		//		return Json(new { Reiniciar = true, Error = true });

		//	var retiro = await _ventas.RetirarEfectivoDeCaja(total, token.UsuarioID);

		//	if (!retiro.Error)
		//		TempData["toast"] = retiro.Mensaje;

		//	return Json(new { retiro.Error, Estatus = retiro.Mensaje });
		//}

		private async Task BitacoraAsync(string accion, VentasDTO venta, string excepcion = "")
		{
			BitacoraContext bitacoraContexto = new(_configuracion.CadenaDeConexionBitacora);
			var _bitacora = new Bitacoras(bitacoraContexto);

			string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

			await _getHelper.SetBitacoraAsync(token, accion, moduloId,
				venta, venta.VentaID.ToString(), directorioBitacora, excepcion);
		}

		private async Task BitacoraAsync(string accion, VentaCanceladaDTO ventaCancelada, Guid usuarioId, string excepcion = "")
		{
			string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

			await _getHelper.SetBitacoraAsync(token, accion, moduloId,
				ventaCancelada, usuarioId.ToString(), directorioBitacora, excepcion);
		}
	}
}

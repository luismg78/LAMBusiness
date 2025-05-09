namespace LAMBusiness.Web.Controllers
{
	using Helpers;
	using Interfaces;
	using LAMBusiness.Backend;
	using LAMBusiness.Contextos;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.ViewFeatures;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Models.ViewModels;
	using Shared.Aplicacion;
	using Shared.Movimiento;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class EntradasController : GlobalController
	{
		private readonly DataContext _context;
		private readonly IGetHelper _getHelper;
		private readonly ICombosHelper _combosHelper;
		private readonly IConverterHelper _converterHelper;
		private readonly IConfiguration _configuration;
		private readonly IDashboard _dashboard;
		private readonly Productos _productos;
		private Guid moduloId = Guid.Parse("B019EBF0-5A25-4CC3-BD72-34FDA134E5C1");

		public EntradasController(DataContext context,
			IGetHelper getHelper,
			ICombosHelper combosHelper,
			IConverterHelper converterHelper,
			IConfiguration configuration,
			IDashboard dashboard)
		{
			_context = context;
			_getHelper = getHelper;
			_combosHelper = combosHelper;
			_converterHelper = converterHelper;
			_configuration = configuration;
			_dashboard = dashboard;
			_productos = new Productos(context);
		}

		public async Task<IActionResult> Index()
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
			{
				return RedirectToAction("Index", "Home");
			}

			var entradas = _context.Entradas
				.Include(e => e.Proveedores)
				.OrderByDescending(e => e.FechaActualizacion)
				.ThenBy(e => e.Proveedores.Nombre);

			var filtro = new Filtro<List<Entrada>>()
			{
				Datos = await entradas.Take(50).ToListAsync(),
				Patron = "",
				PermisoEscritura = permisosModulo.PermisoEscritura,
				PermisoImprimir = permisosModulo.PermisoImprimir,
				PermisoLectura = permisosModulo.PermisoLectura,
				Registros = await entradas.CountAsync(),
				Skip = 0
			};

			return View(filtro);

		}

		public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<Entrada>> filtro)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return null; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
			{
				return null;
			}

			IQueryable<Entrada> query = null;
			if (filtro.Patron != null && filtro.Patron != "")
			{
				var words = filtro.Patron.Trim().ToUpper().Split(' ');
				foreach (var w in words)
				{
					if (w.Trim() != "")
					{
						if (query == null)
						{
							query = _context.Entradas
								.Include(e => e.Proveedores)
								.Where(p => p.Folio.Contains(w) ||
											p.Proveedores.Nombre.Contains(w));
						}
						else
						{
							query = query
								.Include(e => e.Proveedores)
								.Where(p => p.Folio.Contains(w) ||
											p.Proveedores.Nombre.Contains(w));
						}
					}
				}
			}
			if (query == null)
			{
				query = _context.Entradas.Include(e => e.Proveedores);
			}

			filtro.Registros = await query.CountAsync();

			filtro.Datos = await query.OrderByDescending(m => m.FechaActualizacion)
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
							<Filtro<List<Venta>>>(ViewData, filtro)
			};
		}

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

			var entrada = await _getHelper.GetEntradaByIdAsync((Guid)id);

			if (entrada == null)
			{
				TempData["toast"] = "Identificador de la entrada inexistente.";
				return RedirectToAction(nameof(Index));
			}

			var entradaViewModel = await _converterHelper.ToEntradaViewModelAsync(entrada);

			entradaViewModel.PermisoEscritura = permisosModulo.PermisoEscritura;

			return View(entradaViewModel);
		}

		public async Task<IActionResult> Create()
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
			{
				return RedirectToAction(nameof(Index));
			}

			return View(new Entrada());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ProveedorID,Fecha,Folio,Observaciones")] Entrada entrada)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
			{
				return RedirectToAction(nameof(Index));
			}

			if (EntradaAplicada(entrada.EntradaID))
			{
				TempData["toast"] = "Entrada aplicada no se permiten cambios.";
				return RedirectToAction(nameof(Details), new { id = entrada.EntradaID });
			}

			TempData["toast"] = "Falta información en algún campo.";

			if (ModelState.IsValid)
			{
				entrada.Aplicado = false;
				entrada.FechaCreacion = DateTime.Now;
				entrada.FechaActualizacion = DateTime.Now;
				entrada.Folio = entrada.Folio.Trim().ToUpper();
				entrada.Observaciones = entrada.Observaciones == null ? "" : entrada.Observaciones.Trim().ToUpper();
				entrada.UsuarioID = token.UsuarioID;

				try
				{
					_context.Add(entrada);
					await _context.SaveChangesAsync();
					TempData["toast"] = "Los datos de la entrada se almacenaron correctamente.";
					await BitacoraAsync("Alta", entrada);
					return RedirectToAction(nameof(Details), new { id = entrada.EntradaID });
				}
				catch (Exception ex)
				{
					string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
					TempData["toast"] = "Error al guardar entrada, verifique bitácora de errores.";
					await BitacoraAsync("Alta", entrada, excepcion);
				}
			}

			return View(entrada);
		}

		public async Task<IActionResult> Edit(Guid? id)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
			{
				return RedirectToAction(nameof(Index));
			}

			if (id == null)
			{
				TempData["toast"] = "Identificador incorrecto.";
				return RedirectToAction(nameof(Index));
			}

			if (EntradaAplicada((Guid)id))
			{
				TempData["toast"] = "Entrada aplicada no se permiten cambios.";
				return RedirectToAction(nameof(Details), new { id });
			}

			var entrada = await _getHelper.GetEntradaByIdAsync((Guid)id);

			if (entrada == null)
			{
				TempData["toast"] = "Identificador de la entrada inexistente.";
				return RedirectToAction(nameof(Index));
			}

			return View(entrada);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, [Bind("EntradaID,ProveedorID,Fecha,Folio,Observaciones")] Entrada entrada)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
			{
				return RedirectToAction(nameof(Index));
			}

			if (id != entrada.EntradaID)
			{
				TempData["toast"] = "Identificador de la entrada inexistente.";
				return RedirectToAction(nameof(Index));
			}

			if (EntradaAplicada((Guid)id))
			{
				TempData["toast"] = "Entrada aplicada no se permiten cambios.";
				return RedirectToAction(nameof(Details), new { id });
			}

			TempData["toast"] = "Falta información en algún campo.";

			if (ModelState.IsValid)
			{
				var _entrada = await _getHelper.GetEntradaByIdAsync(entrada.EntradaID);

				_entrada.FechaActualizacion = DateTime.Now;
				_entrada.ProveedorID = entrada.ProveedorID;
				_entrada.Folio = entrada.Folio.Trim().ToUpper();
				_entrada.Fecha = entrada.Fecha;
				_entrada.Observaciones = entrada.Observaciones == null ? "" : entrada.Observaciones.Trim().ToUpper();
				_entrada.UsuarioID = token.UsuarioID;

				try
				{
					_context.Update(_entrada);
					await _context.SaveChangesAsync();
					TempData["toast"] = "Los datos de la entrada se actualizaron correctamente.";
					await BitacoraAsync("Actualizar", entrada);
				}
				catch (DbUpdateConcurrencyException ex)
				{
					string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
					await BitacoraAsync("Actualizar", entrada, excepcion);
					if (!EntradaExists(entrada.EntradaID))
					{
						TempData["toast"] = "Identificador de la entrada inexistente.";
						return RedirectToAction(nameof(Index));
					}
					else
					{
						TempData["toast"] = "[Error] Los datos de la entrada no fueron actualizados.";
					}
				}
				catch (Exception ex)
				{
					string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
					TempData["toast"] = "[Error] Los datos de la entrada no fueron actualizados.";
					await BitacoraAsync("Actualizar", entrada, excepcion);
				}
				return RedirectToAction(nameof(Details), new { id = entrada.EntradaID });
			}

			return View(entrada);
		}

		public async Task<IActionResult> Delete(Guid? id)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
			{
				return RedirectToAction(nameof(Index));
			}

			if (id == null)
			{
				TempData["toast"] = "Identificador incorrecto.";
				return RedirectToAction(nameof(Index));
			}

			var entrada = await _context.Entradas
				.Include(e => e.Proveedores)
				.FirstOrDefaultAsync(m => m.EntradaID == id);

			if (entrada == null)
			{
				TempData["toast"] = "Identificador incorrecto, entrada inexistente.";
				return RedirectToAction(nameof(Index));
			}

			if (EntradaAplicada((Guid)id))
			{
				TempData["toast"] = "Entrada aplicada no se permiten cambios.";
				return RedirectToAction(nameof(Details), new { id });
			}

			var entradaDetalle = await _context.EntradasDetalle
				.Where(s => s.EntradaID == id).ToListAsync();

			if (entradaDetalle.Any())
			{
				foreach (var e in entradaDetalle)
				{
					_context.EntradasDetalle.Remove(e);
				}
			}

			_context.Entradas.Remove(entrada);

			try
			{
				await _context.SaveChangesAsync();
				TempData["toast"] = "Los datos de la entrada fueron eliminados correctamente.";
				await BitacoraAsync("Baja", entrada);
			}
			catch (Exception ex)
			{
				string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
				TempData["toast"] = "[Error] Los datos de la entrada no fueron eliminados.";
				await BitacoraAsync("Baja", entrada);
			}

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Apply(Guid? id)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			//cambiar por permiso para aplicar que se debe agregar como acción.
			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
			{
				return RedirectToAction(nameof(Index));
			}

			if (id == null)
			{
				TempData["toast"] = "Identificador incorrecto.";
				return RedirectToAction(nameof(Index));
			}

			if (EntradaAplicada((Guid)id))
			{
				TempData["toast"] = "Entrada aplicada no se permiten cambios.";
				return RedirectToAction(nameof(Details), new { id });
			}

			var entrada = await _getHelper.GetEntradaByIdAsync((Guid)id);

			if (entrada == null)
			{
				TempData["toast"] = "Identificador de la entrada inexistente.";
				return RedirectToAction(nameof(Index));
			}

			entrada.Aplicado = true;
			_context.Update(entrada);

			var detalle = await _getHelper.GetEntradaDetalleByEntradaIdAsync(entrada.EntradaID);
			if (detalle == null)
			{
				TempData["toast"] = "Por favor, ingrese al menos un movimiento.";
				return RedirectToAction(nameof(Details), new { id });
			}

			var existencias = new List<ExistenciaViewModel>();
			Dictionary<Guid, decimal> importePorAlmacen = new Dictionary<Guid, decimal>();

			foreach (var item in detalle)
			{
				Guid _almacenId = (Guid)item.AlmacenID;
				Guid _productoId = (Guid)item.ProductoID;
				decimal _cantidad = (decimal)item.Cantidad;
				decimal _precioCosto = (decimal)item.PrecioCosto;

				if (item.Productos.Unidades.Pieza)
				{
					_cantidad = (int)_cantidad;
				}

				if (item.Productos.Unidades.Paquete)
				{
					if (item.Productos.Paquete == null)
					{
						TempData["toast"] = $"El producto ({item.Productos.Codigo} - {item.Productos.Nombre}) está clasificado como paquete, pero no se encontró el código de la pieza asociada.";
						return RedirectToAction(nameof(Details), new { id });
					}
					_productoId = item.Productos.Paquete.PiezaProductoID;
					_precioCosto = (decimal)item.PrecioCosto / item.Productos.Paquete.CantidadProductoxPaquete;
					_cantidad = item.Productos.Paquete.CantidadProductoxPaquete * _cantidad;
					item.Productos.PrecioCosto = (decimal)item.PrecioCosto;
				}
				item.Productos.PrecioVenta = (decimal)item.PrecioVenta;

				var existencia = existencias
					.FirstOrDefault(e => e.ProductoID == _productoId &&
										e.AlmacenID == _almacenId);

				if (existencia == null)
				{
					existencias.Add(new ExistenciaViewModel()
					{
						AlmacenID = _almacenId,
						ExistenciaEnAlmacen = _cantidad,
						ExistenciaID = Guid.NewGuid(),
						ProductoID = _productoId,
						PrecioCosto = _precioCosto
					});
				}
				else
				{
					existencia.PrecioCosto = (
						(Math.Abs(existencia.ExistenciaEnAlmacen) * existencia.PrecioCosto) +
						(_cantidad * _precioCosto)
						) / (Math.Abs(existencia.ExistenciaEnAlmacen) + _cantidad);
					existencia.ExistenciaEnAlmacen += _cantidad;
				}

				if (!importePorAlmacen.ContainsKey(_almacenId))
				{
					importePorAlmacen.Add(_almacenId, _cantidad * _precioCosto);
				}
				else
				{
					importePorAlmacen[_almacenId] += _cantidad * _precioCosto;
				}
			}

			foreach (KeyValuePair<Guid, decimal> keyValuePair in importePorAlmacen)
			{
				EstadisticaMovimientoViewModel estadisticaMovimiento = new EstadisticaMovimientoViewModel()
				{
					AlmacenID = keyValuePair.Key,
					DB = _context,
					Importe = keyValuePair.Value,
					Movimiento = TipoMovimiento.Entrada
				};

				await _dashboard.GuardarEstadisticaDeMovimientoAsync(estadisticaMovimiento);
			}

			foreach (var item in existencias)
			{
				Guid _almacenId = (Guid)item.AlmacenID;

				var existencia = await _getHelper
					.GetExistenciaByProductoIdAndAlmacenIdAsync(item.ProductoID, _almacenId);
				if (existencia == null)
				{
					_context.Existencias.Add(new Existencia()
					{
						AlmacenID = _almacenId,
						ExistenciaEnAlmacen = item.ExistenciaEnAlmacen,
						ExistenciaID = Guid.NewGuid(),
						ProductoID = item.ProductoID
					});

				}
				else
				{
					existencia.ExistenciaEnAlmacen += item.ExistenciaEnAlmacen;
					_context.Update(existencia);
				}
			}

			var productos = existencias.GroupBy(e => e.ProductoID)
				.Select(g => new
				{
					productoID = g.Key,
					existencia = g.Sum(p => p.ExistenciaEnAlmacen),
					precioCosto = (g.Sum(p => p.ExistenciaEnAlmacen * p.PrecioCosto) / g.Sum(p => p.ExistenciaEnAlmacen))
				})
				.ToList();

			foreach (var p in productos)
			{
				var producto = await _context.Productos
					.FirstOrDefaultAsync(x => x.ProductoID == p.productoID);

				var existenciaActual = await _context.Existencias
					.Where(x => x.ProductoID == p.productoID)
					.SumAsync(e => e.ExistenciaEnAlmacen);

				var pc = ((p.existencia * p.precioCosto) + (producto.PrecioCosto * existenciaActual));

				if (pc == 0)
					producto.PrecioCosto = p.precioCosto;
				else
					producto.PrecioCosto = pc / (p.existencia + existenciaActual);
				_context.Update(producto);
			}

			try
			{
				await _context.SaveChangesAsync();
				TempData["toast"] = "La entrada ha sido aplicada, no podrá realizar cambios en la información.";
				await BitacoraAsync("Aplicar", entrada);
			}
			catch (Exception ex)
			{
				string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
				TempData["toast"] = "Error al aplicar el movimiento, verifique bitácora de errores.";
				await BitacoraAsync("Aplicar", entrada, excepcion);
			}

			return RedirectToAction(nameof(Details), new { id });

		}

		private bool EntradaExists(Guid id)
		{
			return _context.Entradas.Any(e => e.EntradaID == id);
		}

		private bool EntradaAplicada(Guid id)
		{
			return _context.Entradas.Any(e => e.EntradaID == id && e.Aplicado == true);
		}

		//Detalle de movimientos

		public async Task<IActionResult> AddDetails(Guid? id)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
			{
				return RedirectToAction(nameof(Index));
			}

			if (id == null)
			{
				TempData["toast"] = "Identificador incorrecto.";
				return RedirectToAction(nameof(Index));
			}

			if (EntradaAplicada((Guid)id))
			{
				TempData["toast"] = "Entrada aplicada no se permiten cambios.";
				return RedirectToAction(nameof(Details), new { id });
			}

			return View(new EntradaDetalle()
			{
				EntradaID = (Guid)id,
				Cantidad = 0,
				PrecioCosto = 0,
				PrecioVenta = 0,
				AlmacenesDDL = await _combosHelper.GetComboAlmacenesAsync()
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddDetails(EntradaDetalle entradaDetalle)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
			{
				return RedirectToAction(nameof(Index));
			}

			if (entradaDetalle == null)
			{
				TempData["toast"] = "Identificador incorrecto.";
				return RedirectToAction(nameof(Index));
			}

			entradaDetalle.AlmacenesDDL = await _combosHelper.GetComboAlmacenesAsync();

			if (EntradaAplicada(entradaDetalle.EntradaID))
			{
				TempData["toast"] = "Entrada aplicada no se permiten cambios.";
				return RedirectToAction(nameof(Details), new { id = entradaDetalle.EntradaID });
			}

			if (entradaDetalle.AlmacenID == null)
			{
				TempData["toast"] = "El campo almacén es requerido.";
				ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
				return View(entradaDetalle);
			}

			if (entradaDetalle.ProductoID == null)
			{
				TempData["toast"] = "El campo producto es requerido.";
				ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
				return View(entradaDetalle);
			}

			var almacen = await _getHelper.GetAlmacenByIdAsync((Guid)entradaDetalle.AlmacenID);
			var producto = await _productos.ObtenerRegistroPorIdAsync((Guid)entradaDetalle.ProductoID);

			TempData["toast"] = "Falta información en algún campo.";

			ValidarDatosDelProducto(entradaDetalle);

			if (ModelState.IsValid)
			{
				if (almacen == null)
				{
					TempData["toast"] = "El campo almacén es requerido.";
					ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
					return View(entradaDetalle);
				}

				if (producto == null)
				{
					TempData["toast"] = "El campo producto es requerido.";
					ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
					return View(entradaDetalle);
				}

				try
				{
					entradaDetalle.EntradaDetalleID = Guid.NewGuid();

					if (producto.Unidades.Pieza)
					{
						entradaDetalle.Cantidad = (int)entradaDetalle.Cantidad;
					}

					_context.Add(entradaDetalle);

					await _context.SaveChangesAsync();

					TempData["toast"] = "Los datos del producto fueron almacenados correctamente.";
					await BitacoraAsync("Alta", entradaDetalle, entradaDetalle.EntradaID);

					return View(new EntradaDetalle()
					{
						AlmacenID = entradaDetalle.AlmacenID,
						Almacenes = almacen,
						EntradaID = entradaDetalle.EntradaID,
						Cantidad = 0,
						PrecioCosto = 0,
						PrecioVenta = 0,
						AlmacenesDDL = await _combosHelper.GetComboAlmacenesAsync()
					});

				}
				catch (Exception ex)
				{
					string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
					TempData["toast"] = "[Error] Los datos del producto no fueron almacenados.";
					ModelState.AddModelError(string.Empty, "Error al guardar registro");
					await BitacoraAsync("Alta", entradaDetalle, entradaDetalle.EntradaID, excepcion);
				}
			}

			entradaDetalle.Productos = producto;
			return View(entradaDetalle);
		}

		public async Task<IActionResult> EditDetails(Guid? id)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
			{
				return RedirectToAction(nameof(Index));
			}

			if (id == null)
			{
				TempData["toast"] = "Identificador incorrecto.";
				return RedirectToAction(nameof(Index));
			}

			var detalle = await _getHelper.GetEntradaDetalleByIdAsync((Guid)id);

			if (EntradaAplicada(detalle.EntradaID))
			{
				TempData["toast"] = "Entrada aplicada no se permiten cambios.";
				return RedirectToAction(nameof(Details), new { id = detalle.EntradaID });
			}

			detalle.AlmacenesDDL = await _combosHelper.GetComboAlmacenesAsync();
			return View(detalle);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditDetails(EntradaDetalle entradaDetalle)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
			{
				return RedirectToAction(nameof(Index));
			}

			if (entradaDetalle == null)
			{
				TempData["toast"] = "Identificador incorrecto.";
				return RedirectToAction(nameof(Index));
			}

			if (EntradaAplicada(entradaDetalle.EntradaID))
			{
				TempData["toast"] = "Entrada aplicada no se permiten cambios.";
				return RedirectToAction(nameof(Details), new { id = entradaDetalle.EntradaID });
			}

			entradaDetalle.AlmacenesDDL = await _combosHelper.GetComboAlmacenesAsync();

			if (entradaDetalle.AlmacenID == null)
			{
				ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
				return View(entradaDetalle);
			}

			if (entradaDetalle.ProductoID == null)
			{
				ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
				return View(entradaDetalle);
			}

			var almacen = await _getHelper.GetAlmacenByIdAsync((Guid)entradaDetalle.AlmacenID);
			var producto = await _productos.ObtenerRegistroPorIdAsync((Guid)entradaDetalle.ProductoID);

			TempData["toast"] = "Información incompleta, verifique los campos.";

			ValidarDatosDelProducto(entradaDetalle);

			if (ModelState.IsValid)
			{
				if (almacen == null)
				{
					ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
					return View(entradaDetalle);
				}

				if (producto == null)
				{
					ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
					return View(entradaDetalle);
				}

				if (producto.Unidades.Pieza)
				{
					entradaDetalle.Cantidad = (int)entradaDetalle.Cantidad;
				}

				try
				{
					_context.Update(entradaDetalle);

					await _context.SaveChangesAsync();
					TempData["toast"] = "Los datos del producto fueron actualizados correctamente.";
					await BitacoraAsync("Actualizar", entradaDetalle, entradaDetalle.EntradaID);

					return RedirectToAction(nameof(Details), new { id = entradaDetalle.EntradaID });
				}
				catch (Exception ex)
				{
					string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
					TempData["toast"] = "[Error] Los datos del producto no fueron actualizados.";
					ModelState.AddModelError(string.Empty, "Error al actualizar el registro");
					await BitacoraAsync("Actualizar", entradaDetalle, entradaDetalle.EntradaID, excepcion);
				}
			}

			entradaDetalle.Productos = producto;

			return View(entradaDetalle);
		}

		public async Task<IActionResult> DeleteDetails(Guid? id)
		{
			var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
			if (validateToken != null) { return validateToken; }

			if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
			{
				return RedirectToAction(nameof(Index));
			}

			if (id == null)
			{
				TempData["toast"] = "Identificador incorrecto.";
				return RedirectToAction(nameof(Index));
			}

			var detalle = await _getHelper.GetEntradaDetalleByIdAsync((Guid)id);

			if (detalle == null)
			{
				TempData["toast"] = "Identificador de la entrada inexistente.";
				return RedirectToAction(nameof(Index));
			}

			if (EntradaAplicada(detalle.EntradaID))
			{
				TempData["toast"] = "Entrada aplicada no se permiten cambios.";
				return RedirectToAction(nameof(Details), new { id = detalle.EntradaID });
			}

			_context.EntradasDetalle.Remove(detalle);
			try
			{
				await _context.SaveChangesAsync();
				TempData["toast"] = "Los datos del producto fueron eliminados correctamente.";
				await BitacoraAsync("Baja", detalle, detalle.EntradaID);
			}
			catch (Exception ex)
			{
				string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
				TempData["toast"] = "[Error] Los datos del producto no fueron eliminados.";
				await BitacoraAsync("Baja", detalle, detalle.EntradaID, excepcion);
			}

			return RedirectToAction(nameof(Details), new { id = detalle.EntradaID });
		}

		//Bitácora
		private async Task BitacoraAsync(string accion, Entrada entrada, string excepcion = "")
		{
			string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

			await _getHelper.SetBitacoraAsync(token, accion, moduloId,
				entrada, entrada.EntradaID.ToString(), directorioBitacora, excepcion);
		}
		private async Task BitacoraAsync(string accion, EntradaDetalle entradaDetalle, Guid entradaId, string excepcion = "")
		{
			string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

			await _getHelper.SetBitacoraAsync(token, accion, moduloId,
				entradaDetalle, entradaId.ToString(), directorioBitacora, excepcion);
		}

		private void ValidarDatosDelProducto(EntradaDetalle entradaDetalle)
		{
			if (entradaDetalle.PrecioVenta == null || entradaDetalle.PrecioVenta <= 0)
			{
				TempData["toast"] = "Precio de venta incorrecto.";
				ModelState.AddModelError("PrecioVenta", "Precio de venta incorrecto.");
			}

			if (entradaDetalle.Cantidad == null || entradaDetalle.Cantidad <= 0)
			{
				TempData["toast"] = "Cantidad de productos incorrecto.";
				ModelState.AddModelError("Cantidad", "Precio de venta incorrecto.");
			}

			if (entradaDetalle.PrecioCosto == null || entradaDetalle.PrecioCosto < 0)
			{
				TempData["toast"] = "Precio de costo incorrecto.";
				ModelState.AddModelError("PrecioCosto", "Precio de venta incorrecto.");
			}
		}
	}
}

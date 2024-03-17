namespace LAMBusiness.Web.Controllers
{
    using Helpers;
    using Hub;
    using LAMBusiness.Contextos;
    using LAMBusiness.Shared.Movimiento;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Shared.Aplicacion;
    using Shared.Catalogo;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AlmacenesController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;
        private readonly IHubContext<ServerHub> _hubContext;
        private Guid moduloId = Guid.Parse("DA183D55-101E-4A06-9EC3-A1ED5729F0CB");

        public AlmacenesController(DataContext context,
            IConfiguration configuration,
            IGetHelper getHelper,
            IHubContext<ServerHub> hubContext)
        {
            _context = context;
            _configuration = configuration;
            _getHelper = getHelper;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction("Index", "Home");
            }

            var almacenes = _context.Almacenes;

            var filtro = new Filtro<List<Almacen>>()
            {
                Datos = await almacenes.OrderBy(p => p.Nombre).Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoImprimir,
                Registros = await almacenes.CountAsync(),
                Skip = 0
            };

            return View(filtro);
        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<Almacen>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            IQueryable<Almacen> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Almacenes
                                    .Where(p => p.Nombre.Contains(w) ||
                                           p.Descripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Where(p => p.Nombre.Contains(w) ||
                                                p.Descripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Almacenes;
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query.OrderBy(m => m.Nombre)
                .Skip(filtro.Skip)
                .Take(50)
                .ToListAsync();

            filtro.PermisoEscritura = permisosModulo.PermisoEscritura;
            filtro.PermisoImprimir = permisosModulo.PermisoImprimir;
            filtro.PermisoLectura = permisosModulo.PermisoImprimir;

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <Filtro<List<Almacen>>>(ViewData, filtro)
            };
        }

        public async Task<IActionResult> Create()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlmacenID,Nombre,Descripcion")] Almacen almacen, string HubID)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo.";
            if (ModelState.IsValid)
            {
                try
                {
                    almacen.AlmacenID = Guid.NewGuid();
                    _context.Add(almacen);

                    var productos = _context.Productos.Select(p => p.ProductoID);
                    if (productos != null)
                    {
                        await _hubContext.Clients
                            .Client(HubID)
                            .SendAsync("Process", "Guardando información de almacén, por favor espere...");

                        decimal registros = await productos.CountAsync();
                        decimal records = 100 / registros;
                        decimal cont = records;
                        foreach (var item in productos)
                        {
                            _context.Existencias.Add(new Existencia()
                            {
                                AlmacenID = almacen.AlmacenID,
                                ExistenciaEnAlmacen = 0,
                                ExistenciaID = Guid.NewGuid(),
                                ProductoID = item
                            });

                            await _hubContext.Clients
                                .Client(HubID)
                                .SendAsync("ProgressBar", cont);
                            cont += records;
                        }
                    }

                    await _context.SaveChangesAsync();
                    await BitacoraAsync("Alta", almacen);

                    TempData["toast"] = "Los datos del almacén fueron almacenados correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del almacén no fueron almacenados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Alta", almacen, excepcion);
                }
            }

            return View(almacen);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var almacen = await _context.Almacenes.FindAsync(id);
            if (almacen == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            return View(almacen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AlmacenID,Nombre,Descripcion")] Almacen almacen)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != almacen.AlmacenID)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo, verifique.";
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(almacen);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del almacén fueron actualizados correctamente.";
                    await BitacoraAsync("Actualizar", almacen);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!AlmacenExists(almacen.AlmacenID))
                    {
                        TempData["toast"] = "Registro inexistente.";
                    }
                    else
                    {
                        TempData["toast"] = "[Error] Los datos del almacén no fueron actualizados.";
                    }
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", almacen, excepcion);
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del almacén no fueron actualizados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", almacen, excepcion);
                }
            }

            return View(almacen);
        }

        public async Task<IActionResult> Delete(Guid? id, string hubId)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var almacen = await _context.Almacenes
                .Include(a => a.Existencias)
                .FirstOrDefaultAsync(m => m.AlmacenID == id);

            if (almacen == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            bool removeProcess = false;

            if (almacen.Existencias != null)
            {
                decimal existencias = almacen.Existencias.Count(s => s.ExistenciaEnAlmacen > 0);

                if (existencias > 0)
                {
                    TempData["toast"] = $"El almacen no puede ser eliminado, tiene {existencias} producto(s) en existencia(s).";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    await _hubContext.Clients.All
                        .SendAsync("Process", "Eliminando productos asignados al almacén, por favor espere...");

                    decimal records = 100 / (decimal)almacen.Existencias.Count();
                    decimal cont = records;
                    foreach (var item in almacen.Existencias)
                    {
                        _context.Existencias.Remove(item);
                        await _hubContext.Clients.All
                                .SendAsync("ProgressBar", cont);
                        cont += records;
                    }
                    removeProcess = true;
                }
            }

            try
            {
                _context.Almacenes.Remove(almacen);
                await _context.SaveChangesAsync();
                await BitacoraAsync("Baja", almacen);
                TempData["toast"] = "Los datos del almacén fueron eliminados correctamente.";
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos del almacén no fueron eliminados.";
                await BitacoraAsync("Baja", almacen, excepcion);
            }
            finally
            {
                if (removeProcess)
                {
                    await _hubContext.Clients.All
                                .SendAsync("RemoveProcess");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetAlmacenByNameAsync(string almacenNombre)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return new EmptyResult();
            }

            if (almacenNombre == null || almacenNombre == "")
            {
                return new EmptyResult();
            }

            var almacen = await _getHelper.GetAlmacenByNombreAsync(almacenNombre.Trim().ToUpper());
            if (almacen != null)
            {
                return Json(
                    new
                    {
                        almacen.AlmacenID,
                        almacen.Nombre,
                        almacen.Descripcion,
                        error = false
                    });
            }

            return Json(new { error = true, message = "Almacén inexistente" });

        }

        public async Task<IActionResult> GetAlmacenesListAsync(Filtro<List<Almacen>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

            filtro = await _getHelper.GetAlmacenesByPatternAsync(filtro);

            var almacenes = filtro.Datos;
            if (almacenes != null && almacenes.Count > 0)
            {
                return Json(almacenes.Select(m => new
                {
                    Id = m.AlmacenID.ToString(),
                    Text = m.Nombre,
                }));
            }

            return null;
        }

        private bool AlmacenExists(Guid id)
        {
            return _context.Almacenes.Any(e => e.AlmacenID == id);
        }

        private async Task BitacoraAsync(string accion, Almacen almacen, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                almacen, almacen.AlmacenID.ToString(), directorioBitacora, excepcion);
        }

        //private async Task Productos(string HubID)
        //{
        //    int registros = 10000;
        //    decimal records = 100 / (decimal)registros;
        //    decimal cont = records;
        //    await _hubContext.Clients
        //        .Client(HubID)
        //        .SendAsync("Process", "Procesando 10,000 registros, por favor espere...");
        //    Guid marcaId = Guid.Parse("620CEB37-D6A5-4649-9C6E-39581858EFD2");
        //    Guid unidadId = Guid.Parse("401B9552-D654-11E9-8B00-8CDCD47D68A1");
        //    Guid tasaId = Guid.Parse("ACBB8324-7514-4C38-8354-FA5147FA87E6");
        //    for (int x = 1; x <= 10000; x++)
        //    {
        //        _context.Productos.Add(new Producto()
        //        {
        //            Activo = true,
        //            Codigo = $"PROD{x.ToString("00000")}",
        //            MarcaID = marcaId,
        //            PrecioCosto = x,
        //            PrecioVenta = x + 10,
        //            Descripcion = $"PRODUCTO DESCRIPCIÓN {x.ToString("00000")}",
        //            ProductoID = Guid.NewGuid(),
        //            Nombre = $"PRODUCTO {x.ToString("00000")}",
        //            TasaID = tasaId,
        //            UnidadID = unidadId
        //        });

        //        await _hubContext.Clients
        //            .Client(HubID)
        //            .SendAsync("ProgressBar", cont);
        //        cont += records;
        //    }
        //}
    }
}
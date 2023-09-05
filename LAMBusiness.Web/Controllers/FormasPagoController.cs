namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Configuration;
    using Data;
    using Helpers;
    using Shared.Aplicacion;
    using Shared.Catalogo;

    public class FormasPagoController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;
        private Guid moduloId = Guid.Parse("1f1a8a70-239b-432c-afcd-71dff20d042c");

        public FormasPagoController(DataContext context, IConfiguration configuration, IGetHelper getHelper)
        {
            _context = context;
            _configuration = configuration;
            _getHelper = getHelper;
        }

        public async Task<IActionResult> Index()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction("Inicio", "Home");
            }

            var formasPago = _context.FormasPago;

            var filtro = new Filtro<List<FormaPago>>()
            {
                Datos = await formasPago.OrderBy(f => f.Nombre).Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoImprimir,
                Registros = await formasPago.CountAsync(),
                Skip = 0
            };

            return View(filtro);
        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<FormaPago>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            IQueryable<FormaPago> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.FormasPago
                                    .Where(f => f.Nombre.Contains(w));
                        }
                        else
                        {
                            query = query.Where(f => f.Nombre.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.FormasPago;
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
                            <Filtro<List<FormaPago>>>(ViewData, filtro)
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
        public async Task<IActionResult> Create([Bind("FormaPagoID,Nombre,ValorPorDefault")] FormaPago formaPago)
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
                    if (formaPago.ValorPorDefault)
                    {
                        var formaPagoPredeterminada = await _context.FormasPago.FirstOrDefaultAsync(f => f.ValorPorDefault == true);
                        if (formaPagoPredeterminada != null)
                        {
                            formaPagoPredeterminada.ValorPorDefault = false;
                            _context.Update(formaPagoPredeterminada);
                        }
                    }

                    var _formaPago = await _context.FormasPago
                        .OrderByDescending(f => f.FormaPagoID).FirstOrDefaultAsync();
                    if (_formaPago == null)
                    {
                        formaPago.FormaPagoID = 1;
                        formaPago.ValorPorDefault = true;
                    }
                    else
                    {
                        formaPago.FormaPagoID = (byte)(_formaPago.FormaPagoID + 1);
                    }

                    _context.Add(formaPago);
                    await _context.SaveChangesAsync();
                    await BitacoraAsync("Alta", formaPago);
                    TempData["toast"] = "Los datos de la forma de pago fueron almacenados correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos de la forma de pago no fueron almacenados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Alta", formaPago, excepcion);
                }
            }

            return View(formaPago);
        }

        public async Task<IActionResult> Edit(byte? id)
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

            var formaPago = await _context.FormasPago.FindAsync(id);
            if (formaPago == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            return View(formaPago);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("FormaPagoID,Nombre,ValorPorDefault")] FormaPago formaPago)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != formaPago.FormaPagoID)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo, verifique.";
            if (ModelState.IsValid)
            {
                var formaPagoPredeterminada = await _context.FormasPago
                    .FirstOrDefaultAsync(f => f.ValorPorDefault == true && 
                                              f.FormaPagoID != formaPago.FormaPagoID);

                if (formaPago.ValorPorDefault)
                {
                    if (formaPagoPredeterminada != null)
                    {
                        formaPagoPredeterminada.ValorPorDefault = false;
                        _context.Update(formaPagoPredeterminada);
                    }
                }
                else
                {
                    if (formaPagoPredeterminada == null)
                    {
                        formaPago.ValorPorDefault = true;
                    }
                }

                try
                {
                    _context.Update(formaPago);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos de la forma de pago fueron actualizados correctamente.";
                    await BitacoraAsync("Actualizar", formaPago);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!FormaPagoExists(formaPago.FormaPagoID))
                    {
                        TempData["toast"] = "Registro inexistente.";
                    }
                    else
                    {
                        TempData["toast"] = "[Error] Los datos de la forma de pago no fueron actualizados.";
                    }
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", formaPago, excepcion);
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos de la forma de pago no fueron actualizados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", formaPago, excepcion);
                }
            }

            return View(formaPago);
        }

        public async Task<IActionResult> Delete(byte? id)
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

            var formaPago = await _context.FormasPago
                               .FirstOrDefaultAsync(f => f.FormaPagoID == id);

            if (formaPago == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }
            
            bool eliminar = true;

            if (formaPago.ValorPorDefault)
            {
                var formaPagoPredeterminada = await _context.FormasPago
                    .Where(f => f.FormaPagoID != formaPago.FormaPagoID)
                    .OrderBy(f => f.FormaPagoID).FirstOrDefaultAsync();
                if (formaPagoPredeterminada != null)
                {
                    formaPagoPredeterminada.ValorPorDefault = true;
                    _context.Update(formaPagoPredeterminada);
                }
                else
                {
                    if (formaPago.FormaPagoID != 1)
                    {
                        _context.FormasPago.Add(new FormaPago()
                        {
                            ValorPorDefault = true,
                            Nombre = "EFECTIVO",
                            FormaPagoID = 1
                        });
                    }
                    else
                    {
                        eliminar = false;
                    }
                }
            }

            try
            {
                if (eliminar)
                {
                    _context.FormasPago.Remove(formaPago);
                    await _context.SaveChangesAsync();
                    await BitacoraAsync("Baja", formaPago);
                    TempData["toast"] = "Los datos de la forma de pago fueron eliminados correctamente.";
                }
                else { 
                    TempData["toast"] = "Los datos de la forma de pago no fueron eliminados, debe existir al menos un registro.";
                }
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos de la forma de pago no fueron eliminados.";
                await BitacoraAsync("Baja", formaPago, excepcion);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FormaPagoExists(byte id)
        {
            return _context.FormasPago.Any(f => f.FormaPagoID == id);
        }

        private async Task BitacoraAsync(string accion, FormaPago formaPago, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                formaPago, formaPago.FormaPagoID.ToString(), directorioBitacora, excepcion);
        }
    }
}

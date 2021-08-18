namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Data;
    using Helpers;
    using Models.ViewModels;
    using Shared.Catalogo;
    using Shared.Movimiento;

    public class ProductosController : GlobalController
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IGetHelper _getHelper;
        private readonly IConfiguration _configuration;
        private Guid moduloId = Guid.Parse("A549419C-89BD-49CE-BA93-4D73AFBA37CE");

        public ProductosController(DataContext context, 
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IGetHelper getHelper,
            IConfiguration configuration)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _getHelper = getHelper;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction("Inicio", "Menu");
            }

            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            var dataContext = _context.Productos
                .Include(p => p.Marcas)
                .Include(p => p.TasasImpuestos)
                .Include(p => p.Unidades)
                .Include(p => p.Paquete)
                .OrderBy(p => p.Codigo);

            return View(dataContext);
        }

        public async Task<IActionResult> _AddRowsNextAsync(string searchby, int skip)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            IQueryable<Producto> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Productos                                    
                                    .Where(p => p.Codigo.Contains(w) ||
                                                p.ProductoNombre.Contains(w) ||
                                                p.ProductoDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Where(p => p.Codigo.Contains(w) ||
                                                p.ProductoNombre.Contains(w) ||
                                                p.ProductoDescripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Productos;
            }

            var productos = await query
                .Include(p => p.Marcas)
                .Include(p => p.TasasImpuestos)
                .Include(p => p.Unidades)
                .Include(p => p.Paquete)
                .OrderBy(p => p.ProductoNombre)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<Producto>>(ViewData, productos)
            };
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Marcas)
                .Include(p => p.TasasImpuestos)
                .Include(p => p.Unidades)
                .Include(p => p.Paquete)
                .Include(p => p.Existencias)
                .ThenInclude(p => p.Almacenes)
                .FirstOrDefaultAsync(m => m.ProductoID == id);
            if (producto == null)
            {
                return NotFound();
            }

            var productoDetailsViewModel = await _converterHelper.ToProductosDetailsViewModelAsync(producto);
            
            return View(productoDetailsViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            var productoViewModel = new ProductoViewModel() {
                UnidadesDDL = await _combosHelper.GetComboUnidadesAsync(),
                TasasImpuestosDDL = await _combosHelper.GetComboTasaImpuestosAsync()
            };

            if(productoViewModel.TasasImpuestosDDL.Count() <= 1)
            {
                ModelState.AddModelError(string.Empty, "No existe tasa de impuestos para asignar a productos.");
                return RedirectToAction(nameof(Create), "TasasImpuestos");
            }

            return View(productoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoViewModel productoViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            await ValidateFieldsAsync(productoViewModel);
            if (ModelState.IsValid)
            {
                var producto = await _converterHelper.ToProductoAsync(productoViewModel, true);
                _context.Add(producto);

                bool paqueteExists = PaqueteExists(producto.ProductoID);

                if(producto.Paquete != null)
                {
                    if (paqueteExists)
                        _context.Update(producto.Paquete);
                    else
                        _context.Add(producto.Paquete);
                }
                else
                {
                    if (paqueteExists)
                    {
                        _context.Remove(producto.Paquete);
                    }
                }

                var almacen = await _context.Almacenes.ToListAsync();
                if(almacen != null)
                {
                    foreach(var a in almacen)
                    {
                        _context.Add(new Existencia(){
                            AlmacenID = a.AlmacenID,
                            ExistenciaEnAlmacen = (decimal)0,
                            ExistenciaID = Guid.NewGuid(),
                            ProductoID = producto.ProductoID
                        });
                    }
                }

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = producto.ProductoID});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            productoViewModel.TasasImpuestosDDL = await _combosHelper.GetComboTasaImpuestosAsync();
            productoViewModel.UnidadesDDL = await _combosHelper.GetComboUnidadesAsync();
            return View(productoViewModel);
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
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Marcas)
                .FirstOrDefaultAsync(p => p.ProductoID == id);

            if (producto == null)
            {
                return NotFound();
            }

            var productoViewModel = await _converterHelper.ToProductosViewModelAsync(producto);

            return View(productoViewModel);
        }   

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductoViewModel productoViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != productoViewModel.ProductoID)
            {
                return NotFound();
            }

            await ValidateFieldsAsync(productoViewModel);

            if (ModelState.IsValid)
            {
                try
                {
                    var producto = await _converterHelper.ToProductoAsync(productoViewModel, false);

                    producto.PrecioCosto = _context.Productos
                        .Where(p => p.ProductoID == producto.ProductoID)
                        .Select(p => p.PrecioCosto).FirstOrDefault();

                    _context.Update(producto);
                    
                    var paquete = producto.Paquete;
                    bool paqueteExists = PaqueteExists(producto.ProductoID);

                    if (paquete != null)
                    {
                        if (paqueteExists)
                            _context.Update(paquete);
                        else
                            _context.Add(paquete);
                    }
                    else
                    {
                        paquete = await _getHelper.GetPaqueteByIdAsync(producto.ProductoID);
                        if (paquete != null) {
                            if (paqueteExists)
                            {
                                _context.Remove(paquete);
                            }
                        }

                        if (!producto.Activo)
                        {
                            var packages = await _context.Paquetes
                                .Where(p => p.PiezaProductoID == producto.ProductoID)
                                .ToListAsync();
                            if (packages != null)
                            {
                                foreach (var pack in packages)
                                {
                                    var productoPack = await _getHelper
                                        .GetProductByIdAsync(pack.ProductoID);
                                    if (productoPack != null)
                                    {
                                        if (productoPack.Activo)
                                        {
                                            productoPack.Activo = producto.Activo;
                                            _context.Update(productoPack);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = producto.ProductoID});

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!ProductoExists(productoViewModel.ProductoID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                        return View();
                    }
                }
            }
            
            productoViewModel.TasasImpuestosDDL = await _combosHelper.GetComboTasaImpuestosAsync();
            productoViewModel.UnidadesDDL = await _combosHelper.GetComboUnidadesAsync();

            return View(productoViewModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Marcas)
                .Include(p => p.Unidades)
                .Include(p => p.Existencias)
                .FirstOrDefaultAsync(p => p.ProductoID == id);

            if (producto == null)
            {
                return NotFound();
            }

            if (producto.Unidades.Paquete) {
                var paquete = await _context.Paquetes.FindAsync(producto.ProductoID);
                if (paquete != null) {
                    _context.Remove(paquete);
                }
            }else
            {
                var piezaAsignada = await _context.Paquetes
                    .Where(p => p.PiezaProductoID == producto.ProductoID)
                    .ToListAsync();
                if (piezaAsignada.Count > 0)
                {
                    ModelState.AddModelError(string.Empty, $"El producto no se puede eliminar, porque está asignado a {piezaAsignada.Count} paquete(s).");
                    return RedirectToAction(nameof(Index));
                }
            }

            if(producto.Existencias.Count > 0)
            {
                foreach (var existencia in producto.Existencias)
                {
                    if (existencia.ExistenciaEnAlmacen > 0)
                    {
                        ModelState.AddModelError(string.Empty, $"El producto no se puede eliminar, porque tiene {producto.Existencias.Count} en existencia(s).");
                        return RedirectToAction(nameof(Index));
                    }
                }
                foreach (var existencia in producto.Existencias)
                {
                    _context.Existencias.Remove(existencia);
                }
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetMarca(string nombreMarca)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

            if (nombreMarca == null || nombreMarca == "")
            {
                return null;
            }

            var marca = await _getHelper.GetMarcaByNombreAsync(nombreMarca);
            if (marca != null)
            {
                return Json(
                    new
                    {
                        marca.MarcaID,
                        marca.MarcaNombre,
                        marca.MarcaDescripcion,
                        error = false
                    });
            }

            return Json(new { error = true, message = "Marca inexistente" });

        }

        public async Task<IActionResult> GetMarcas(string pattern, int? skip)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

            if (pattern == null || pattern == "" || skip == null)
            {
                return null;
            }

            var marcas = await _getHelper.GetMarcasByPatternAsync(pattern, (int)skip);

            return new PartialViewResult
            {
                ViewName = "_GetMarcas",
                ViewData = new ViewDataDictionary
                            <List<Marca>>(ViewData, marcas)
            };
        }

        private bool PaqueteExists(Guid id)
        {
            return _context.Paquetes.Any(e => e.ProductoID == id);
        }

        private bool ProductoExists(Guid id)
        {
            return _context.Productos.Any(e => e.ProductoID == id);
        }

        private async Task ValidateFieldsAsync(ProductoViewModel productoViewModel)
        {
            productoViewModel.Unidades = await _getHelper.GetUnidadByIdAsync((Guid)productoViewModel.UnidadID);
            //validate if register exist with that value
            var codeexist = await _context.Productos
                .Include(p => p.Unidades)
                .FirstOrDefaultAsync(p => p.Codigo == productoViewModel.Codigo &&
                                     p.ProductoID != productoViewModel.ProductoID);
            if (codeexist != null)
            {
                ModelState.AddModelError("Codigo", "Ya existe un producto con este código.");
            }
            //validate field Tasa de Impuesto 
            if (productoViewModel.TasaID == null || productoViewModel.TasaID == Guid.Empty)
            {
                ModelState.AddModelError("TasaID", "El campo Tasa de Impuesto es requerido");
            }
            //validate field Unidad
            if (productoViewModel.UnidadID == null || productoViewModel.UnidadID == Guid.Empty)
            {
                ModelState.AddModelError("UnidadID", "El campo Unidad es requerido");
            }
            else
            {
                //if field unidad have value, then, validate fields CodigoPieza and Cantidad
                if (productoViewModel.Unidades.Paquete)
                {
                    if (string.IsNullOrEmpty(productoViewModel.CodigoPieza))
                    {
                        ModelState.AddModelError("CodigoPieza", "El campo Unidad es requerido");
                    }
                    //if field CodigoPieza have value, then, validate if product exist
                    var codepieceexist = await _context.Productos
                        .Include(p => p.Unidades)
                        .FirstOrDefaultAsync(p => p.Codigo == productoViewModel.CodigoPieza);
                    if (codepieceexist == null)
                    {
                        ModelState.AddModelError("CodigoPieza", "Código de producto inexistente.");
                    }
                    else if (!codepieceexist.Activo)
                    {
                        ModelState.AddModelError("CodigoPieza", "Producto no disponible (ver detalle).");
                    }
                    else
                    {
                        //validate that producto be piece
                        if (codepieceexist.Unidades.Paquete)
                        {
                            ModelState.AddModelError("CodigoPieza", "El producto no puede ser caja.");
                        }
                        else
                        {
                            //validate package and piece be whole pieces
                            if (productoViewModel.Unidades.Pieza)
                            {
                                if (!codepieceexist.Unidades.Pieza)
                                {
                                    ModelState.AddModelError("CodigoPieza", "No coincide con la unidad del paquete.");
                                }
                            }
                            //validate package and piece be bulk pieces
                            else if (!productoViewModel.Unidades.Pieza)
                            {
                                if (codepieceexist.Unidades.Pieza)
                                {
                                    ModelState.AddModelError("CodigoPieza", "No coincide con la unidad del paquete.");
                                }
                            }
                        }
                    }

                    if (productoViewModel.CantidadProductoxPaquete == null ||
                        productoViewModel.CantidadProductoxPaquete <= 0)
                    {
                        ModelState.AddModelError("CantidadProductoxPaquete", "El campo Unidad es requerido");
                    }
                }
            }
        }
    }
}

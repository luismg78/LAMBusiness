namespace LAMBusiness.Web.Controllers
{
    using Data;
    using Helpers;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Models.ViewModels;
    using Shared.Catalogo;
    using Shared.Movimiento;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductosController : GlobalController
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IGetHelper _getHelper;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private Guid moduloId = Guid.Parse("A549419C-89BD-49CE-BA93-4D73AFBA37CE");

        public ProductosController(DataContext context, 
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IGetHelper getHelper,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _getHelper = getHelper;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
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
            
            string directorio = _configuration.GetValue<string>("DirectorioImagenProducto");
            string directorioProducto = Path.Combine(directorio, producto.ProductoID.ToString(), "md");

            if(Directory.Exists(directorioProducto))
            { 
                DirectoryInfo directory = new DirectoryInfo(directorioProducto);
                string _file = "";
                List<Guid> images = new List<Guid>();
                foreach(var file in directory.GetFiles())
                {
                    _file = Path.GetFileNameWithoutExtension(file.ToString());
                    images.Add(Guid.Parse(_file));
                }
                productoDetailsViewModel.ProductoImages = images;
            }
            
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

            try
            {
                string directorio = _configuration.GetValue<string>("DirectorioImagenProducto");
                string directorioProducto = Path.Combine(directorio, producto.ProductoID.ToString());
                if (Directory.Exists(directorioProducto))
                {
                    Directory.Delete(directorioProducto, true);
                }

                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                TempData["toast"] = "El producto ha sido eliminado satisfactoriamente.";

            }
            catch (Exception)
            {
                throw;
            }

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

        public async Task<FileContentResult> GetProductImages(Guid productoId, Guid imageId, string tipo)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "home");
            if (validateToken != null) { return null; }

            string directoryProduct = _configuration.GetValue<string>("DirectorioImagenProducto");
            string directoryProductImage = Path.Combine(directoryProduct, productoId.ToString());
            string ruta = Path.Combine(directoryProductImage, tipo, $"{imageId}.png");

            if (!System.IO.File.Exists(ruta))
            {
                ruta = Path.Combine(_webHostEnvironment.WebRootPath, "images", "productos", tipo, "ShoppingCart.png");
            }

            return _converterHelper.ToImageBase64(ruta);
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

        //Images products

        public async Task<IActionResult> AddProductImage(Guid id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            var producto = await _context.Productos.FindAsync(id);
            if(producto == null)
            {
                TempData["toast"] = "Identificador del producto inexistente";
                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }

        [HttpPost]
        public async Task<JsonResult> AddProductImage(Guid id, string img)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return Json(new { Estatus = "Lo sentimos, no tiene privilegios para crear imágenes.", Error = true });
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return Json(new { Estatus = "Error: Identificador del producto inexistente", Error = true });
            }

            string path = "";
            int tamaño = 0;

            string productoId = $"{Guid.NewGuid()}.png";
            string directorioImagenProducto = _configuration.GetValue<string>("DirectorioImagenProducto");
            string directorioImagenProductoID = $"{directorioImagenProducto}//{producto.ProductoID}//";

            if (!Directory.Exists(directorioImagenProducto))
            {
                Directory.CreateDirectory(directorioImagenProducto);
            }
            if (!Directory.Exists(directorioImagenProductoID))
            {
                Directory.CreateDirectory(directorioImagenProductoID);
            }
            if (!Directory.Exists($"{directorioImagenProductoID}//sm//"))
            {
                Directory.CreateDirectory($"{directorioImagenProductoID}//sm//");
            }
            if (!Directory.Exists($"{directorioImagenProductoID}//md//"))
            {
                Directory.CreateDirectory($"{directorioImagenProductoID}//md//");
            }
            if (!Directory.Exists($"{directorioImagenProductoID}//lg//"))
            {
                Directory.CreateDirectory($"{directorioImagenProductoID}//lg//");
            }

            if(!System.IO.File.Exists($"{directorioImagenProductoID}//lg//{producto.ProductoID}.png"))
            {
                productoId = $"{producto.ProductoID}.png";
            }

            try
            {
                var index = img.IndexOf(',') + 1;
                img = img.Substring(index);

                for (byte x = 1; x <= 3; x++)
                {
                    switch (x)
                    {
                        case 1:
                            path = Path.Combine($"{directorioImagenProductoID}//sm//", productoId);
                            tamaño = 95;
                            break;
                        case 2:
                            path = Path.Combine($"{directorioImagenProductoID}//md//", productoId);
                            tamaño = 380;
                            break;
                        case 3:
                            path = Path.Combine($"{directorioImagenProductoID}//lg//", productoId);
                            tamaño = 760;
                            break;
                    }

                    byte[] bi = _converterHelper.UploadImageBase64(img, tamaño);
                    using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(bi, 0, bi.Length);
                    }
                }
                TempData["toast"] = "Imagen del producto creada";
                return Json(new { Error = false });
            }
            catch (Exception)
            {
                return Json(new { Estatus = "Error: Imagen del producto no creada" + "[" + path + "]", Error = true });
            }
        }

        public async Task<IActionResult> DeleteProductImage(Guid productoId, Guid imageId)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Details), new { id = productoId });
            }

            string directorioImagenProducto = _configuration.GetValue<string>("DirectorioImagenProducto");
            string directorioImagenProductoID = $"{directorioImagenProducto}//{productoId}//";

            if (Directory.Exists(directorioImagenProducto))
            {
                if (Directory.Exists(directorioImagenProductoID))
                {
                    if (Directory.Exists($"{directorioImagenProductoID}//sm//"))
                    {
                        if(System.IO.File.Exists($"{directorioImagenProductoID}//sm//{imageId}.png"))
                        {
                            System.IO.File.Delete($"{directorioImagenProductoID}//sm//{imageId}.png");
                        }
                    }
                    if (Directory.Exists($"{directorioImagenProductoID}//md//"))
                    {
                        if (System.IO.File.Exists($"{directorioImagenProductoID}//md//{imageId}.png"))
                        {
                            System.IO.File.Delete($"{directorioImagenProductoID}//md//{imageId}.png");
                        }
                    }
                    if (Directory.Exists($"{directorioImagenProductoID}//lg//"))
                    {
                        if (System.IO.File.Exists($"{directorioImagenProductoID}//lg//{imageId}.png"))
                        {
                            System.IO.File.Delete($"{directorioImagenProductoID}//lg//{imageId}.png");
                        }
                    }
                }
            }

            if(productoId == imageId)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo($"{directorioImagenProductoID}//lg//");
                string _file = "";
                foreach(var file in directoryInfo.GetFiles())
                {
                    _file = file.Name;
                    break;
                }
                if (System.IO.File.Exists($"{directorioImagenProductoID}//sm//{_file}"))
                {
                    System.IO.File.Move($"{directorioImagenProductoID}//sm//{_file}", $"{directorioImagenProductoID}//sm//{productoId}.png");
                }
                if (System.IO.File.Exists($"{directorioImagenProductoID}//md//{_file}"))
                {
                    System.IO.File.Move($"{directorioImagenProductoID}//md//{_file}", $"{directorioImagenProductoID}//md//{productoId}.png");
                }
                if (System.IO.File.Exists($"{directorioImagenProductoID}//lg//{_file}"))
                {
                    System.IO.File.Move($"{directorioImagenProductoID}//lg//{_file}", $"{directorioImagenProductoID}//lg//{productoId}.png");
                }
            }

            TempData["toast"] = "La imagen del producto ha sido eliminada.";
            return RedirectToAction(nameof(Details), new { id = productoId });

        }

        public async Task<IActionResult> MainProductImage(Guid productoId, Guid imageId)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Details), new { id = productoId });
            }

            if (productoId == imageId)
            {
                TempData["toast"] = "La imagen del producto ha sido asignada como principal.";
                return RedirectToAction(nameof(Details), new { id = productoId });
            }

            string directorioImagenProducto = _configuration.GetValue<string>("DirectorioImagenProducto");
            string directorioImagenProductoID = $"{directorioImagenProducto}//{productoId}//";
            Guid idNew = Guid.NewGuid();

            if (Directory.Exists(directorioImagenProducto))
            {
                if (Directory.Exists(directorioImagenProductoID))
                {
                    if (Directory.Exists($"{directorioImagenProductoID}//sm//"))
                    {
                        if (System.IO.File.Exists($"{directorioImagenProductoID}//sm//{productoId}.png"))
                        {
                            System.IO.File.Move($"{directorioImagenProductoID}//sm//{productoId}.png", $"{directorioImagenProductoID}//sm//{idNew}.png");
                        }
                        System.IO.File.Move($"{directorioImagenProductoID}//sm//{imageId}.png", $"{directorioImagenProductoID}//sm//{productoId}.png");
                    }
                    if (Directory.Exists($"{directorioImagenProductoID}//md//"))
                    {
                        if (System.IO.File.Exists($"{directorioImagenProductoID}//md//{productoId}.png"))
                        {
                            System.IO.File.Move($"{directorioImagenProductoID}//md//{productoId}.png", $"{directorioImagenProductoID}//md//{idNew}.png");
                        }
                        System.IO.File.Move($"{directorioImagenProductoID}//md//{imageId}.png", $"{directorioImagenProductoID}//md//{productoId}.png");
                    }
                    if (Directory.Exists($"{directorioImagenProductoID}//lg//"))
                    {
                        if (System.IO.File.Exists($"{directorioImagenProductoID}//lg//{productoId}.png"))
                        {
                            System.IO.File.Move($"{directorioImagenProductoID}//lg//{productoId}.png", $"{directorioImagenProductoID}//lg//{idNew}.png");
                        }
                        System.IO.File.Move($"{directorioImagenProductoID}//lg//{imageId}.png", $"{directorioImagenProductoID}//lg//{productoId}.png");
                    }
                }
            }

            TempData["toast"] = "La imagen del producto ha sido asignada como principal.";
            return RedirectToAction(nameof(Details), new { id = productoId });

        }
    }
}

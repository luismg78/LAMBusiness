﻿namespace LAMBusiness.Web.Controllers
{
    using DocumentFormat.OpenXml.InkML;
    using Helpers;
    using Interfaces;
    using LAMBusiness.Backend;
    using LAMBusiness.Contextos;
    using LAMBusiness.Shared.DTO.Movimiento;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Models.ViewModels;
    using Shared.Aplicacion;
    using Shared.Movimiento;
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

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
                return RedirectToAction(nameof(Index));

            var resultado = await _ventas.Inicializar(token.UsuarioID);
            if (resultado.Error)
            {
                TempData["toast"] = resultado.Mensaje;
                return RedirectToAction("Index", "Movimiento");
            }

            return View(resultado.Datos);
        }

        public async Task<IActionResult> Agregar(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return Json(new { Reiniciar = true, Error = true });
            }

            var agregar = await _ventas.Agregar(id, token.UsuarioID);
            return Json(new { Estatus = agregar.Mensaje, agregar.Reiniciar, agregar.Error });
        }

        public async Task<IActionResult> Aplicar(Guid? id, decimal importe)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
                return Json(new { Reiniciar = true, Error = true });

            var venta = await _ventas.Aplicar(id, token.UsuarioID, importe);
            if (venta.Error)
            {
                await BitacoraAsync("Aplicar", venta.Datos, venta.Excepcion);
                return Json(new { Estatus = "Venta no realizada.", Error = true });
            }

            await BitacoraAsync("Aplicar", venta.Datos);
            return PartialView(venta.Datos);
        }

        public async Task<IActionResult> CerrarVentas()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
                return Json(new { Reiniciar = true, Error = true });

            var corteDeCaja = await _ventas.CerrarVentas(token.UsuarioID);
            if (corteDeCaja.Error)
                return Json(new { Error = true, Estatus = corteDeCaja.Mensaje });

            return PartialView(corteDeCaja.Datos);
        }

        public async Task<IActionResult> RecuperarVentaPorId(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
                return RedirectToAction(nameof(Index));

            var venta = await _ventas.RecuperarVentaPorId(id, token.UsuarioID);
            if (venta.Error)
            {
                TempData["toast"] = venta.Mensaje;
                return RedirectToAction("Index", "Movimiento");
            }


            return View(nameof(Index), venta.Datos);
        }

        public async Task<IActionResult> RecuperarVenta()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return Json(new { Reiniciar = true, Error = true });
            }

            var filtro = await _ventas.RecuperarVenta(token.UsuarioID);
            if (filtro.Error)
                return Json(new { Estatus = filtro.Mensaje, Error = true });

            return PartialView(filtro.Datos);
        }

        public async Task<IActionResult> ObtenerProductoPorCodigo(Guid? id, string codigo, decimal cantidad)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            var resultado = await _ventas.ObtenerProducto(id, token.UsuarioID, codigo, cantidad);

            if (resultado.Error)
            {
                switch (resultado.Mensaje.Trim().ToLower())
                {
                    case "buscarproducto":
                        return Json(new { BuscarProducto = true, Error = true, Reiniciar = false });
                    case "reiniciar":
                        TempData["toast"] = "Identificador de la venta incorrecto.";
                        return Json(new { Reiniciar = true, Error = true });
                    default:
                        return Json(new { Estatus = resultado.Mensaje, Error = true, Reiniciar = true });
                }
            }

            return PartialView(resultado.Datos);
        }

        public async Task<IActionResult> CancelarVenta(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return Json(new { Reiniciar = true, Error = true });
            }

            var ventaCancelada = await _ventas.CancelarVenta(id, token.UsuarioID);
            if (ventaCancelada.Error)
            {
                await BitacoraAsync("CancelarVenta", ventaCancelada.Datos, token.UsuarioID, ventaCancelada.Mensaje);
                return Json(new { Estatus = "Cancelación no realizada.", Error = true });
            }

            await BitacoraAsync("CancelarVenta", ventaCancelada.Datos, token.UsuarioID);
            return Json(new { Error = false });
        }

        public async Task<IActionResult> RetirarEfectivoDeCaja(decimal total)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
                return Json(new { Reiniciar = true, Error = true });

            var retiro = await _ventas.RetirarEfectivoDeCaja(total, token.UsuarioID);

            if (!retiro.Error)
                TempData["toast"] = retiro.Mensaje;
                
            return Json(new { retiro.Error, Estatus = retiro.Mensaje });
        }

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

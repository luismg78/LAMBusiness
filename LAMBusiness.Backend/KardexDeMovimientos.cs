namespace LAMBusiness.Backend
{
    using LAMBusiness.Contextos;
    using LAMBusiness.Shared.Aplicacion;
    using LAMBusiness.Shared.Kardex;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class KardexDeMovimientos
    {
        private readonly Configuracion _configuracion;

        public KardexDeMovimientos(Configuracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task<KardexDeProducto> KardexDeProductos(Filtro<KardexDeProducto> filtro)
        {
            KardexDeProducto datos = filtro.Datos;
            List<KardexDeProductoDetalle> query;
            var inventarios = Task.Run(() => InventarioDeProductos(datos.ProductoID, datos.AlmacenID));
            var entradas = Task.Run(() => EntradasDeProductos(datos.ProductoID, datos.AlmacenID));
            var salidas = Task.Run(() => SalidasDeProductos(datos.ProductoID, datos.AlmacenID));
            var ventas = Task.Run(() => VentasDeProductos(datos.ProductoID, datos.AlmacenID));
            var ventasCanceladas = Task.Run(() => VentasCanceladasDeProductos(datos.ProductoID));

            await Task.WhenAll(inventarios, entradas, salidas, ventas, ventasCanceladas);

            query = inventarios.Result.Union(entradas.Result).ToList();
            query = query.Union(salidas.Result).ToList();
            query = query.Union(ventas.Result).ToList();
            query = query.Union(ventasCanceladas.Result).ToList();

            List<string> tipoDeMovimientos = new();
            if (datos.Inventarios)
                tipoDeMovimientos.Add("Inventario");
            if (datos.Entradas)
                tipoDeMovimientos.Add("Entrada");
            if (datos.Salidas)
                tipoDeMovimientos.Add("Salida");
            if (datos.Ventas)
                tipoDeMovimientos.Add("Venta");
            if (datos.VentasCanceladas)
                tipoDeMovimientos.Add("Venta Cancelada");

            var movimientos = query
                .Where(q => tipoDeMovimientos.Contains(q.Movimiento))
                .OrderBy(q => q.Fecha)
                .ToList();
            //.Take(100)
            //.Skip(0)

            decimal existencia = 0;
            foreach (var item in movimientos)
            {
                switch (item.Movimiento.ToLower())
                {
                    case "inventario":
                        if (item.Aplicado)
                            existencia = (decimal)item.Cantidad!;
                        else
                            item.IconoColor = "var(--bs-gray-300)";
                        break;
                    case "entrada":
                        if (item.Aplicado)
                            existencia += (decimal)item.Cantidad!;
                        else
                            item.IconoColor = "var(--bs-gray-300)";
                        break;
                    case "salida":
                    case "venta":
                        if (item.Aplicado)
                            existencia -= (decimal)item.Cantidad!;
                        else
                            item.IconoColor = "var(--bs-gray-300)";
                        break;
                }
                item.Existencia = existencia;
            }

            datos.KardexDeProductoDetalle = movimientos.OrderByDescending(q => q.Fecha).ToList();

            datos.Existencia = await ExistenciasDeProductos(datos.ProductoID, datos.AlmacenID);

            return datos;
        }

        private async Task<decimal> ExistenciasDeProductos(Guid productoId, Guid almacenId)
        {
            using var _contexto = new DataContext(_configuracion);
            return await (from e in _contexto.Existencias
                          where e.ProductoID == productoId
                             && e.AlmacenID == almacenId
                          select e.ExistenciaEnAlmacen)
                               .FirstOrDefaultAsync();
        }

        private List<KardexDeProductoDetalle> InventarioDeProductos(Guid productoId, Guid almacenId)
        {
            using var contexto = new DataContext(_configuracion);
            var datos = (from e in contexto.Inventarios
                         join d in contexto.InventariosDetalle on e.InventarioID equals d.InventarioID
                         join u in contexto.Usuarios on e.UsuarioID equals u.UsuarioID
                         where d.ProductoID == productoId
                            && d.AlmacenID == almacenId
                         select new KardexDeProductoDetalle()
                         {
                             Movimiento = "Inventario",
                             Aplicado = e.Aplicado,
                             Icono = "fas fa-exchange-alt",
                             IconoColor = "var(--bs-warning)",
                             Cantidad = d.CantidadInventariada,
                             Fecha = e.FechaActualizacion,
                             Estatus = e.Aplicado ? "Aplicado" : "No Aplicado",
                             Usuario = u.NombreCompleto
                         }).ToList();

            return datos;
        }

        private List<KardexDeProductoDetalle> EntradasDeProductos(Guid productoId, Guid almacenId)
        {
            using var contexto = new DataContext(_configuracion);
            var datos = (from e in contexto.Entradas
                         join d in contexto.EntradasDetalle on e.EntradaID equals d.EntradaID
                         join u in contexto.Usuarios on e.UsuarioID equals u.UsuarioID
                         where d.ProductoID == productoId
                            && d.AlmacenID == almacenId
                         select new KardexDeProductoDetalle()
                         {
                             Movimiento = "Entrada",
                             Aplicado = e.Aplicado,
                             Icono = "fas fa-arrow-alt-circle-down",
                             IconoColor = "var(--bs-info)",
                             Cantidad = d.Cantidad,
                             Fecha = e.FechaActualizacion,
                             Estatus = e.Aplicado ? "Aplicado" : "No Aplicado",
                             Usuario = u.NombreCompleto
                         }).ToList();

            return datos;
        }

        private List<KardexDeProductoDetalle> SalidasDeProductos(Guid productoId, Guid almacenId)
        {
            using var contexto = new DataContext(_configuracion);
            var datos = (from e in contexto.Salidas
                         join d in contexto.SalidasDetalle on e.SalidaID equals d.SalidaID
                         join u in contexto.Usuarios on e.UsuarioID equals u.UsuarioID
                         where d.ProductoID == productoId
                            && d.AlmacenID == almacenId
                         select new KardexDeProductoDetalle()
                         {
                             Movimiento = "Salida",
                             Aplicado = e.Aplicado,
                             Icono = "fas fa-arrow-alt-circle-up",
                             IconoColor = "var(--lm-primary)",
                             Cantidad = d.Cantidad,
                             Fecha = e.FechaActualizacion,
                             Estatus = e.Aplicado ? "Aplicado" : "No Aplicado",
                             Usuario = u.NombreCompleto
                         }).ToList();
            return datos;
        }

        private List<KardexDeProductoDetalle> VentasDeProductos(Guid productoId, Guid almacenId)
        {
            using var contexto = new DataContext(_configuracion);
            var datos = (from e in contexto.Ventas
                         join d in contexto.VentasDetalle on e.VentaID equals d.VentaID
                         join u in contexto.Usuarios on e.UsuarioID equals u.UsuarioID
                         where d.ProductoID == productoId
                            && e.AlmacenID == almacenId
                         select new KardexDeProductoDetalle()
                         {
                             Movimiento = "Venta",
                             Aplicado = true,
                             Icono = "fas fa-shopping-cart",
                             IconoColor = "var(--bs-success)",
                             Cantidad = d.Cantidad,
                             Fecha = e.Fecha,
                             Estatus = "Aplicado",
                             Usuario = u.NombreCompleto
                         }).ToList();
            return datos;
        }

        private List<KardexDeProductoDetalle> VentasCanceladasDeProductos(Guid productoId)
        {
            using var contexto = new DataContext(_configuracion);
            var datos = (from e in contexto.VentasCanceladas
                         join d in contexto.VentasCanceladasDetalle on e.VentaCanceladaID equals d.VentaCanceladaID
                         join u in contexto.Usuarios on e.UsuarioID equals u.UsuarioID
                         where d.ProductoID == productoId
                         //&& d.AlmacenID == almacenId
                         select new KardexDeProductoDetalle()
                         {
                             Movimiento = "Venta Cancelada",
                             Aplicado = true,
                             Icono = "fas fa-shopping-cart",
                             IconoColor = "var(--bs-gray-300)",
                             Cantidad = d.Cantidad,
                             Fecha = e.Fecha,
                             Estatus = e.VentaCompleta ? "Cancelado en venta completa" : "Cancelación parcial",
                             Usuario = u.NombreCompleto
                         }).ToList();
            return datos;
        }
    }
}

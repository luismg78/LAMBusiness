namespace LAMBusiness.Backend
{
	using LAMBusiness.Contextos;
	using LAMBusiness.Shared.Aplicacion;
	using LAMBusiness.Shared.Kardex;
	using Microsoft.EntityFrameworkCore;
	using System.Linq;
	using System.Threading.Tasks;

	public class KardexDeMovimientos(Configuracion configuracion)
	{
		private readonly Configuracion Configuracion = configuracion;

		public async Task<List<KardexDeProductoDetalle>> KardexDeProductos(Guid productoId, Guid almacenId)
		{
			var inventarios = InventarioDeProductos(productoId, almacenId);
			var entradas = EntradasDeProductos(productoId, almacenId);
			var salidas = SalidasDeProductos(productoId, almacenId);
			var ventas = VentasDeProductos(productoId, almacenId);

			await Task.WhenAll(inventarios, entradas, salidas, ventas);

			var kardex = ventas.Result.Union(entradas.Result.Union(salidas.Result.Union(inventarios.Result)))
				.OrderByDescending(e => e.Fecha)
				.ToList();
			return kardex;
		}

		private async Task<List<KardexDeProductoDetalle>> InventarioDeProductos(Guid productoId, Guid almacenId)
		{
			var contexto = new DataContext(Configuracion);
			var datos = await (from e in contexto.Inventarios
							   join d in contexto.InventariosDetalle on e.InventarioID equals d.InventarioID
							   join u in contexto.Usuarios on e.UsuarioID equals u.UsuarioID
							   where d.ProductoID == productoId
								  && d.AlmacenID == almacenId
							   select new KardexDeProductoDetalle
							   {
								   Folio = "",
								   Movimiento = "Inventario",
								   Aplicado = e.Aplicado,
								   Icono = "fas fa-exchange-alt",
								   IconoColor = "var(--bs-warning)",
								   Cantidad = d.CantidadInventariada,
								   Precio = d.PrecioCosto,
								   Fecha = e.FechaActualizacion,
								   Estatus = e.Aplicado ? "Aplicado" : "No Aplicado",
								   Usuario = u.NombreCompleto
							   }).OrderByDescending(q => q.Fecha)
						 .Skip(0)
						 .Take(100)
						 .ToListAsync();

			return datos;
		}

		private async Task<List<KardexDeProductoDetalle>> EntradasDeProductos(Guid productoId, Guid almacenId)
		{
			var contexto = new DataContext(Configuracion);
			var datos = await (from e in contexto.Entradas
							   join d in contexto.EntradasDetalle on e.EntradaID equals d.EntradaID
							   join u in contexto.Usuarios on e.UsuarioID equals u.UsuarioID
							   where d.ProductoID == productoId
								  && d.AlmacenID == almacenId
							   select new KardexDeProductoDetalle
							   {
								   Folio = e.Folio,
								   Movimiento = "Entrada",
								   Aplicado = e.Aplicado,
								   Icono = "fas fa-arrow-alt-circle-down",
								   IconoColor = "var(--bs-info)",
								   Cantidad = d.Cantidad,
								   Precio = d.PrecioCosto,
								   Fecha = e.FechaActualizacion,
								   Estatus = e.Aplicado ? "Aplicado" : "No Aplicado",
								   Usuario = u.NombreCompleto,
								   Url = $"/entradas/details/{e.EntradaID}"
							   }).OrderByDescending(q => q.Fecha)
						 .Skip(0)
						 .Take(100)
						 .ToListAsync();
			return datos;
		}

		private async Task<List<KardexDeProductoDetalle>> SalidasDeProductos(Guid productoId, Guid almacenId)
		{
			var contexto = new DataContext(Configuracion);
			var datos = await (from e in contexto.Salidas
							   join d in contexto.SalidasDetalle on e.SalidaID equals d.SalidaID
							   join u in contexto.Usuarios on e.UsuarioID equals u.UsuarioID
							   where d.ProductoID == productoId
								  && d.AlmacenID == almacenId
							   select new KardexDeProductoDetalle
							   {
								   Folio = e.Folio,
								   Movimiento = "Salida",
								   Aplicado = e.Aplicado,
								   Icono = "fas fa-arrow-alt-circle-up",
								   IconoColor = "var(--lm-primary)",
								   Cantidad = d.Cantidad,
								   Precio = d.PrecioCosto,
								   Fecha = e.FechaActualizacion,
								   Estatus = e.Aplicado ? "Aplicado" : "No Aplicado",
								   Usuario = u.NombreCompleto,
								   Url = $"/salidas/details/{e.SalidaID}"
							   }).OrderByDescending(q => q.Fecha)
						 .Skip(0)
						 .Take(100)
						 .ToListAsync();
			return datos;
		}

		private async Task<List<KardexDeProductoDetalle>> VentasDeProductos(Guid productoId, Guid almacenId)
		{
			var contexto = new DataContext(Configuracion);
			var datos = await (from e in contexto.Ventas
							   join d in contexto.VentasDetalle on e.VentaID equals d.VentaID
							   join u in contexto.Usuarios on e.UsuarioID equals u.UsuarioID
							   where d.ProductoID == productoId
								  && e.AlmacenID == almacenId
							   select new KardexDeProductoDetalle
							   {
								   Folio = e.Folio.ToString(),
								   Movimiento = "Venta",
								   Aplicado = true,
								   Icono = "fas fa-shopping-cart",
								   IconoColor = "var(--bs-success)",
								   Cantidad = d.Cantidad,
								   Precio = d.PrecioVenta,
								   Fecha = e.Fecha,
								   Estatus = "Aplicado",
								   Usuario = u.NombreCompleto,
								   Url = $"/ventas/details/{e.VentaID}"
							   }).OrderByDescending(q => q.Fecha)
						 .Skip(0)
						 .Take(100)
						 .ToListAsync();
			return datos;
		}
	}
}

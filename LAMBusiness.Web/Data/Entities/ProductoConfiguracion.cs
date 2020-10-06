namespace LAMBusiness.Web.Data.Entities
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Catalogo;

    public class ProductoConfiguracion : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.HasData(
                GetProductoPieza(),
                GetProductoPaquetePieza(),
                GetProductoKilogramo(),
                GetProductoPaqueteKilogramo()
            ); ;
        }

        //Productos pieza
        private Producto GetProductoPieza() 
        {
            return new Producto
            {
                Activo = true,
                Codigo = "PIEZA",
                PrecioCosto = 10,
                PrecioVenta = 15,
                ProductoDescripcion = "INFORMACIÓN DE APOYO (MODIFICAR INFORMACIÓN).",
                ProductoNombre = "PRODUCTO PIEZA (MODIFICAR)",
                ProductoID = Guid.Parse("DE7C7462-69BA-4343-A328-012F48F013AF"),
                TasaID = Guid.Parse("89E98CD2-85DF-401A-9F1D-308027A75558"),
                UnidadID = Guid.Parse("401b9552-d654-11e9-8b00-8cdcd47d68a1")
            };
        }
        private object GetProductoPaquetePieza()
        {
            return new Producto
            {
                Activo = true,
                Codigo = "PAQUETE",
                PrecioCosto = 90,
                PrecioVenta = 120,
                ProductoDescripcion = "INFORMACIÓN DE APOYO (MODIFICAR INFORMACIÓN).",
                ProductoNombre = "PAQUETE PIEZA (MODIFICAR)",
                ProductoID = Guid.Parse("94C079EE-1FBE-4CAE-9A16-443261DD0D60"),
                TasaID = Guid.Parse("89E98CD2-85DF-401A-9F1D-308027A75558"),
                UnidadID = Guid.Parse("6c9c7801-d654-11e9-8b00-8cdcd47d68a1")
            };
        }

        //Productos kilogramos
        private Producto GetProductoKilogramo()
        {
            return new Producto
            {
                Activo = true,
                Codigo = "PIEZAKG",
                PrecioCosto = (decimal)17.99,
                PrecioVenta = (decimal)22.99,
                ProductoDescripcion = "INFORMACIÓN DE APOYO (MODIFICAR INFORMACIÓN).",
                ProductoNombre = "PRODUCTO KG (MODIFICAR)",
                ProductoID = Guid.Parse("38ABF163-90AD-4D67-9BAB-E5867D2715CF"),
                TasaID = Guid.Parse("ACBB8324-7514-4C38-8354-FA5147FA87E6"),
                UnidadID = Guid.Parse("826671fc-d654-11e9-8b00-8cdcd47d68a1")
            };
        }
        private object GetProductoPaqueteKilogramo()
        {
            return new Producto
            {
                Activo = true,
                Codigo = "PAQUETEKG",
                PrecioCosto = (decimal)149.99,
                PrecioVenta = (decimal)199.99,
                ProductoDescripcion = "INFORMACIÓN DE APOYO (MODIFICAR INFORMACIÓN).",
                ProductoNombre = "PAQUETE KG (MODIFICAR)",
                ProductoID = Guid.Parse("435A7B4D-1347-4282-9B06-3792ED1A99C4"),
                TasaID = Guid.Parse("ACBB8324-7514-4C38-8354-FA5147FA87E6"),
                UnidadID = Guid.Parse("95b850ec-d654-11e9-8b00-8cdcd47d68a1")
            };
        }
    }
}

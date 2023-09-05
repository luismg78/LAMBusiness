namespace LAMBusiness.Web.Data
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Data.Entities;
    using Shared.Aplicacion;
    using Shared.Catalogo;
    using Shared.Contacto;
    using Shared.Movimiento;
    using Shared.Dashboard.Entidades;

    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
        }

        #region Aplicación
        //public DbSet<Bitacora> Bitacora { get; set; }
        //public DbSet<BitacoraExcepciones> BitacoraExcepciones { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<Sesion> Sesiones { get; set; }
        #endregion
        #region Catálogo
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Almacen> Almacenes { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<EstadoCivil> EstadosCiviles { get; set; }
        public DbSet<FormaPago> FormasPago { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Paquete> Paquetes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Puesto> Puestos { get; set; }
        public DbSet<TasaImpuesto> TasasImpuestos { get; set; }
        public DbSet<Unidad> Unidades { get; set; }

        #endregion
        #region Contacto
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ClienteContacto> ClienteContactos { get; set; }
        public DbSet<DatoPersonal> DatosPersonales { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<ProveedorContacto> ProveedorContactos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioModulo> UsuariosModulos { get; set; }

        #endregion
        #region Dashboard
        public DbSet<EstadisticasMovimientosDiario> EstadisticasMovimientosDiario { get; set; }
        public DbSet<EstadisticasMovimientosMensual> EstadisticasMovimientosMensual { get; set; }
        #endregion
        #region Movimiento
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<EntradaDetalle> EntradasDetalle { get; set; }
        public DbSet<Existencia> Existencias { get; set; }
        public DbSet<Salida> Salidas { get; set; }
        public DbSet<SalidaDetalle> SalidasDetalle { get; set; }
        public DbSet<SalidaTipo> SalidasTipo { get; set; }
        public DbSet<RetiroCaja> RetirosCaja { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<VentaCancelada> VentasCanceladas { get; set; }
        public DbSet<VentaCanceladaDetalle> VentasCanceladasDetalle { get; set; }
        public DbSet<VentaCierre> VentasCierre { get; set; }
        public DbSet<VentaCierreDetalle> VentasCierreDetalle { get; set; }
        public DbSet<VentaDetalle> VentasDetalle { get; set; }
        public DbSet<VentaImporte> VentasImportes { get; set; }
        public DbSet<VentaNoAplicada> VentasNoAplicadas { get; set; }
        public DbSet<VentaNoAplicadaDetalle> VentasNoAplicadasDetalle { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) {
                foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }

            //Catálogo (Atributos con valores únicos)
            modelBuilder.Entity<Estado>()
                .HasIndex(e => new { e.Clave })
                .IsUnique(true);

            modelBuilder.Entity<Producto>()
                .HasIndex(p => new { p.Codigo })
                .IsUnique(true);

            modelBuilder.Entity<Marca>()
                .HasIndex(m => new { m.Nombre })
                .IsUnique(true);

            modelBuilder.Entity<Puesto>()
                .HasIndex(p => new { p.Nombre })
                .IsUnique(true);

            modelBuilder.Entity<TasaImpuesto>()
                .HasIndex(t => new { t.Nombre })
                .IsUnique(true);

            modelBuilder.Entity<Unidad>()
                .HasIndex(u => new { u.Nombre })
                .IsUnique(true);

            //Contacto (Atributos con valores únicos)
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => new { c.RFC })
                .IsUnique(true);
            
            modelBuilder.Entity<DatoPersonal>()
                .HasIndex(c => new { c.CURP })
                .IsUnique(true);

            modelBuilder.Entity<Proveedor>()
                .HasIndex(p => new { p.RFC })
                .IsUnique(true);

            //Dashboard (Atributos con valores únicos)
            modelBuilder.Entity<EstadisticasMovimientosDiario>()
                .HasIndex(e => new { e.Fecha })
                .IsUnique(true);

            //Precargar entidades, ver configuraciones en /Data/Entities
            //Catálogo
            modelBuilder.ApplyConfiguration(new AlmacenConfiguracion());
            modelBuilder.ApplyConfiguration(new EstadoConfiguracion());
            modelBuilder.ApplyConfiguration(new EstadoCivilConfiguracion());
            modelBuilder.ApplyConfiguration(new GeneroConfiguracion());
            modelBuilder.ApplyConfiguration(new MarcaConfiguracion());
            modelBuilder.ApplyConfiguration(new MunicipioConfiguracion());
            modelBuilder.ApplyConfiguration(new UnidadConfiguracion());
            modelBuilder.ApplyConfiguration(new PuestoConfiguracion());
            modelBuilder.ApplyConfiguration(new TasaImpuestoConfiguracion());
            modelBuilder.ApplyConfiguration(new ProductoConfiguracion());
            modelBuilder.ApplyConfiguration(new PaqueteConfiguracion());
            //Contacto
            modelBuilder.ApplyConfiguration(new AdministradorConfiguracion());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguracion());
            //Movimiento
            modelBuilder.ApplyConfiguration(new ExistenciaConfiguracion());

            //Configuración
            modelBuilder.ApplyConfiguration(new ModuloConfiguracion());

            base.OnModelCreating(modelBuilder);
        }
    }
}

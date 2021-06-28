namespace LAMBusiness.Web.Data
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Data.Entities;
    using Models.Entities;
    using Shared.Catalogo;
    using Shared.Contacto;
    using Shared.Movimiento;

    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
        }

        #region Catálogo
        public DbSet<Almacen> Almacenes { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<EstadoCivil> EstadosCiviles { get; set; }
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
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<ProveedorContacto> ProveedorContactos { get; set; }

        #endregion
        #region Movimiento
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<EntradaDetalle> EntradasDetalle { get; set; }
        public DbSet<Existencia> Existencias { get; set; }
        #endregion
        public DbSet<Modulo> Modulos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) {
                foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }

            //Catálogo (Atributos con valores únicos)
            modelBuilder.Entity<Estado>()
                .HasIndex(e => new { e.EstadoClave })
                .IsUnique(true);

            modelBuilder.Entity<Producto>()
                .HasIndex(p => new { p.Codigo })
                .IsUnique(true);

            modelBuilder.Entity<Marca>()
                .HasIndex(m => new { m.MarcaNombre })
                .IsUnique(true);

            modelBuilder.Entity<Puesto>()
                .HasIndex(p => new { p.PuestoNombre })
                .IsUnique(true);

            modelBuilder.Entity<TasaImpuesto>()
                .HasIndex(t => new { t.Tasa })
                .IsUnique(true);

            modelBuilder.Entity<Unidad>()
                .HasIndex(u => new { u.UnidadNombre })
                .IsUnique(true);

            //Contacto (Atributos con valores únicos)
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => new { c.RFC })
                .IsUnique(true);
            
            modelBuilder.Entity<Colaborador>()
                .HasIndex(c => new { c.CURP })
                .IsUnique(true);

            modelBuilder.Entity<Proveedor>()
                .HasIndex(p => new { p.RFC })
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
            modelBuilder.ApplyConfiguration(new ColaboradorConfiguracion());
            //Movimiento
            modelBuilder.ApplyConfiguration(new ExistenciaConfiguracion());
        }
    }
}

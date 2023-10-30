namespace LAMBusiness.Contextos
{
    using Microsoft.EntityFrameworkCore;
    using Shared.Aplicacion;

    public class BitacoraContext: DbContext
    {
        private readonly string _cadenaDeConexion;

        public BitacoraContext(string cadenaDeConexion)
        {
            _cadenaDeConexion = cadenaDeConexion;
        }

        public BitacoraContext(Configuracion configuracion)
        {
            _cadenaDeConexion = configuracion.CadenaDeConexionBitacora;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_cadenaDeConexion);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        #region Aplicación
        public DbSet<Bitacora> Bitacora { get; set; }
        public DbSet<BitacoraExcepciones> BitacoraExcepciones { get; set; }
        #endregion
    }
}

namespace LAMBusiness.Contextos
{
    using Microsoft.EntityFrameworkCore;
    using Shared.Aplicacion;

    public class BitacoraContext: DbContext
    {
        public BitacoraContext(DbContextOptions<BitacoraContext> options) : base(options)
        {
        }

        #region Aplicación
        public DbSet<Bitacora> Bitacora { get; set; }
        public DbSet<BitacoraExcepciones> BitacoraExcepciones { get; set; }
        #endregion
    }
}

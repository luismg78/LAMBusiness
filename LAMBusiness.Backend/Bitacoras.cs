using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.DTO.Bitacora;
using Newtonsoft.Json;

namespace LAMBusiness.Backend
{
    public class Bitacoras
    {
        private readonly BitacoraContext _contexto;

        public Bitacoras(BitacoraContext contexto)
        {
            _contexto = contexto;
        }

        public async Task AgregarAsync(Token token, string accion, Guid moduloId, object clase,
           string parametroId, string directorio, string excepcion = "")
        {
            Guid bitacoraId = Guid.NewGuid();
            Bitacora bitacora = new()
            {
                Accion = accion.Trim(),
                AccionRealizada = excepcion == "" ? true : false,
                BitacoraID = bitacoraId,
                Fecha = DateTime.Now,
                Hostname = token.HostName,
                IPPrivada = token.IPPrivada,
                IPPublica = token.IPPublica,
                ModuloID = moduloId,
                ParametrosJson = JsonConvert.SerializeObject(clase, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }),
                ParametroID = parametroId,
                UsuarioID = token.UsuarioID
            };

            _contexto.Bitacora.Add(bitacora);

            if (!string.IsNullOrEmpty(excepcion))
            {
                _contexto.BitacoraExcepciones.Add(new BitacoraExcepciones()
                {
                    BitacoraID = bitacoraId,
                    Excepcion = excepcion
                });
            }

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                BitacoraExcepcionDTO bitacoraExcepcion = new()
                {
                    Accion = accion.Trim(),
                    AccionRealizada = excepcion == "" ? true : false,
                    BitacoraID = bitacoraId,
                    ErrorAlGuardarBitacoraDB = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString(),
                    Excepcion = excepcion,
                    Fecha = DateTime.Now,
                    Hostname = token.HostName,
                    IPPrivada = token.IPPrivada,
                    IPPublica = token.IPPublica,
                    ModuloID = moduloId,
                    ParametrosJson = JsonConvert.SerializeObject(clase, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }),
                    ParametroID = parametroId,
                    UsuarioID = token.UsuarioID
                };
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }
                string file = Path.Combine(directorio, $"{bitacoraId}.txt");
                File.WriteAllText(file, JsonConvert.SerializeObject(bitacoraExcepcion));
            }
        }
    }
}

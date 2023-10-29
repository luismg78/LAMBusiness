using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.Contacto;
using LAMBusiness.Shared.DTO.Sesion;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace LAMBusiness.Backend
{
    public class Sesiones
    {
        private readonly DataContext _contexto;

        public Sesiones(DataContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<Resultado> CambiarPassword(ChangePasswordDTO changePassword)
        {
            Resultado resultado = new();

            if (changePassword.PasswordEncrypt != changePassword.ConfirmPasswordEncrypt)
            {
                resultado.Error = true;
                resultado.Mensaje = "Contraseñas distintas, verifique su información";
                return resultado;
            }

            var usuario = await _contexto.Usuarios.FindAsync(changePassword.UsuarioID);
            if (usuario == null)
            {
                resultado.Error = true;
                resultado.Mensaje = "Identificador incorrecto, usuario inexistente.";
                return resultado;
            }

            try
            {
                usuario.Password = Encrypt(changePassword.PasswordEncrypt);
                _contexto.Update(usuario);
                usuario.CambiarPassword = false;

                await _contexto.SaveChangesAsync();
                resultado.Mensaje = "Su contraseña ha sido actualizada con éxito.";
            }
            catch (Exception)
            {
                resultado.Error = true;
                resultado.Mensaje = "Error al actualizar el cambio de contraseña.";
            }

            changePassword.ConfirmPassword = "";
            changePassword.ConfirmPasswordEncrypt = "";
            changePassword.Password = "";
            changePassword.PasswordEncrypt = "";

            return resultado;
        }

        public async Task<Resultado<Usuario>> IniciarSesion(InicioDeSesionDTO inicioDeSesion)
        {
            Resultado<Usuario> resultado = new();

            string email = inicioDeSesion.Email.Trim().ToLower();

            Usuario? usuario = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                resultado.Error = true;
                resultado.Mensaje = "Correo electrónico inexistente, verifique";
                return resultado;
            }

            var pwd = Encrypt(inicioDeSesion.Password);
            if (usuario.Password != pwd)
            {
                resultado.Error = true;
                resultado.Mensaje = "Credenciales Incorrectas, verifique";
                return resultado;
            }

            if (!usuario.Activo)
            {
                resultado.Error = true;
                resultado.Mensaje = "Cuenta inactiva, verifique su información con el administrador del sistema.";
                return resultado;
            }

            DateTime fecha = DateTime.Now;

            if (fecha < usuario.FechaInicio)
            {
                resultado.Error = true;
                resultado.Mensaje = $"Cuenta no habilitada (Fecha de inicio: {usuario.FechaInicio.ToShortDateString()}).";
                return resultado;
            }
            if (fecha > usuario.FechaTermino)
            {
                resultado.Error = true;
                resultado.Mensaje = $"Cuenta no habilitada (Fecha de término: {usuario.FechaInicio.ToShortDateString()}).";
                return resultado;
            }

            usuario.FechaUltimoAcceso = fecha;
            try
            {
                _contexto.Update(usuario);
                await _contexto.SaveChangesAsync();
                resultado.Datos = usuario;
            }
            catch (Exception)
            {
                resultado.Error = true;
                resultado.Mensaje = "Error en el servidor al evaluar sus credenciales.";
            }

            return resultado;
        }

        public async Task<Resultado> ResetearPassword(Guid id)
        {
            Resultado resultado = new();
            var rand = new Random();
            int pwd = rand.Next(1000, 10000);

            var usuario = await _contexto.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                resultado.Error = true;
                resultado.Mensaje = "Contraseñas distintas, verifique su información";
                return resultado;
            }

            string pwdSha512 = GenerateSHA512String(pwd.ToString());
            try
            {
                usuario.Password = Encrypt(pwdSha512);
                usuario.CambiarPassword = true;
                _contexto.Update(usuario);
                await _contexto.SaveChangesAsync();

                resultado.Mensaje = $"La contraseña ha sido reiniciada con éxito. Contraseña temporal: {pwd}";
            }
            catch (Exception)
            {
                resultado.Error = true;
                resultado.Mensaje = "Error al reiniciar la contraseña.";
            }

            return resultado;
        }
        private static string Decrypt(string password)
        {
            string strDecrypt = "";
            string EncryptionKey = "#9C03451C-1CA7-4870-8D80-E5AAEF2C2462!";
            byte[] cipherBytes = Convert.FromBase64String(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    strDecrypt = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return strDecrypt;
        }
        private static string Encrypt(string password)
        {
            string strEncrypt = "";
            string EncryptionKey = "#9C03451C-1CA7-4870-8D80-E5AAEF2C2462!";
            byte[] clearBytes = Encoding.Unicode.GetBytes(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    strEncrypt = Convert.ToBase64String(ms.ToArray());
                }
            }
            return strEncrypt;
        }
        public static string GenerateSHA512String(string inputString)
        {
            var message = Encoding.UTF8.GetBytes(inputString);
            using (var sha512 = SHA512.Create())
            {
                string hex = "";

                var hashValue = sha512.ComputeHash(message);
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }
                return hex;
            }
        }
    }
}

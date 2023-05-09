namespace LAMBusiness.Web.Helpers
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class CriptografiaHelper : ICriptografiaHelper
    {
        public string Decrypt(string password)
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
        public string Encrypt(string password)
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
        public string GenerateSHA512String(string inputString)
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

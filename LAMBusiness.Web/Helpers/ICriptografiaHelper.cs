namespace LAMBusiness.Web.Helpers
{
    public interface ICriptografiaHelper
    {
        string Decrypt(string password);
        string Encrypt(string password);
        string GenerateSHA512String(string inputString);
    }
}
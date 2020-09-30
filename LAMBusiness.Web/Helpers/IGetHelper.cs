using System;
using System.Threading.Tasks;

namespace LAMBusiness.Web.Helpers
{
    public interface IGetHelper
    {
        Task<string> GetCodigoProductoAsync(Guid productoID);
        Task<Guid> GetProductoIDAsync(string codigo);
    }
}
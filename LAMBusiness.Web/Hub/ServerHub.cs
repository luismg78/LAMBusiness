namespace LAMBusiness.Web.Hub
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    public class ServerHub: Hub
    {
        public string GetConnectionUserId() => Context.ConnectionId;
    }
}

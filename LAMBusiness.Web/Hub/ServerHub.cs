namespace LAMBusiness.Web.Hub
{
    using Microsoft.AspNetCore.SignalR;

    public class ServerHub: Hub
    {
        public string GetConnectionUserId () => Context.ConnectionId;
    }
}

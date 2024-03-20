using Microsoft.AspNetCore.SignalR;

namespace Taxi.Server.Order.Microservice.Api.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        public virtual string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}

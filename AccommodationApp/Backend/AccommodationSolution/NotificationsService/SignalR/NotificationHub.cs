using Microsoft.AspNetCore.SignalR;
using NotificationsService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationsService.SignalR
{
    public class NotificationHub : Hub
    {
        private IConnectionManager _manager;

        public NotificationHub(IConnectionManager manager)
        {
            _manager = manager;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public string CreateConnection()
        {
            var httpContext = this.Context.GetHttpContext();
            var pathSplit = httpContext.Request.Path.ToString().Split("/");
            var idx = pathSplit.Count<string>() - 1;
            var userId = pathSplit[idx];

            _manager.AddConnection(userId, Context.ConnectionId);

            return Context.ConnectionId;
        }
    }
}

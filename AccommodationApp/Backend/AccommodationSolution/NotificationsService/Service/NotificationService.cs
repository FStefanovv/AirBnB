using Grpc.Core;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationsService.Model;
using NotificationsService.RabbitMQ;
using NotificationsService.Repository;
using NotificationsService.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationsService.Service
{
    public class NotificationService : NotificationGRPCService.NotificationGRPCServiceBase, IConsumer<INotification>
    {
        private NotificationRepository _repository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IConnectionManager _connectionManager;


        public NotificationService(NotificationRepository repository, IHubContext<NotificationHub> hubContext, IConnectionManager connectionManager)
        {
            _repository = repository;
            _hubContext = hubContext;
            _connectionManager = connectionManager; 
        }

        public List<Notification> GetNotifications(string userId)
        {
            return _repository.GetUserNotifications(userId);
        }

        public async Task Consume(ConsumeContext<INotification> context)
        {
            var data = context.Message;

            Notification notification = new Notification { UserId = data.UserId, NotificationContent = data.NotificationContent, CreatedAt = data.CreatedAt };
            _repository.Create(notification);

            HashSet<string> connections = _connectionManager.GetConnections(data.UserId);

            /*
            if (connections == null || connections.Count == 0)
                throw new Exception("no connections");
            */

            if(connections!=null && connections.Count>0)
            {
                foreach(var conn in connections)
                {
                    try
                    {
                        await _hubContext.Clients.Client(conn).SendAsync("NewNotification", notification);
                    }
                    catch {}
                }
            }
        }
    }
}

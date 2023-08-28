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


        public NotificationService(NotificationRepository repository, IHubContext<NotificationHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
        }

        public List<Notification> GetNotifications(string userId)
        {
            return _repository.GetUserNotifications(userId);
        }

        public async Task Consume(ConsumeContext<INotification> context)
        {
            var data = context.Message;

            Notification notification = new Notification { UserId = data.UserId, NotificationContent = data.NotificationContent, CreatedAt = DateTime.Now.ToString("MM/dd/yyyy HH:mm") };
            _repository.Create(notification);
            await _hubContext.Clients.All.SendAsync("NewNotification", notification);
        }
    }
}

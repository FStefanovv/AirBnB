using System;

namespace NotificationsService.RabbitMQ
{
    public class INotification
    {
        public string UserId { get; set; }
        public string NotificationContent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


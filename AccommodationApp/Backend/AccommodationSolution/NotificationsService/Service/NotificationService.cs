using Grpc.Core;
using NotificationsService.Model;
using NotificationsService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationsService.Service
{
    public class NotificationService : NotificationGRPCService.NotificationGRPCServiceBase
    {
        private NotificationRepository _repository;

        public NotificationService(NotificationRepository repository)
        {
            _repository = repository;
        }

        public List<Notification> GetNotifications(string userId)
        {
            return _repository.GetUserNotifications(userId);
        }

        //CreateNotification(NotificationData) returns(NotificationCreated);
        public override Task<NotificationCreated> CreateNotification(NotificationData data, ServerCallContext context)
        {
            //using var scope = _tracer.BuildSpan("UpdateRequestsPostUserDeletion").StartActive(true);
            Notification notification = new Notification { UserId = data.UserId, NotificationContent = data.NotificationContent };
            _repository.Create(notification);
            return Task.FromResult(new NotificationCreated
            {
                Success = true
            });
        }

    }
}

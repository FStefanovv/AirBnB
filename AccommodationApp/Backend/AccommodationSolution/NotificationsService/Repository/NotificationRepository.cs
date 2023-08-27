using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationsService.Model;

namespace NotificationsService.Repository
{
    public class NotificationRepository
    {
        private readonly IDbContext _context;
        private IMongoCollection<Notification> _notifications;

        public NotificationRepository(IDbContext context)
        {
            _context = context;
            _notifications = _context.GetCollection<Notification>("notifications");
        }


        public List<Notification> GetUserNotifications(string userId)
        {
            return _notifications.Find(not => not.UserId == userId).ToList();
        }


        public void Create(Notification notification)
        {
            _notifications.InsertOne(notification);
        }

    }
}

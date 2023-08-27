using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationsService.Model
{
    public class Notification
    {
        public string UserId { get; set; }
        public string NotificationContent { get; set; }


        public Notification() { }
    }
}

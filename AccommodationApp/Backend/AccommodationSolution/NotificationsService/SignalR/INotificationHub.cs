using NotificationsService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationsService.SignalR
{
    public interface IHubClient
    {
        public Task Send(Notification notification);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationsService.SignalR
{
    public interface IConnectionManager
    {
        void AddConnection(string userId, string connectionId);

        void RemoveConnection(string connectionId);

        HashSet<string> GetConnections(string userId);

        IEnumerable<string> OnlineUsers { get;  }
    }
}

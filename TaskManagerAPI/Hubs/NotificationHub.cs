using Microsoft.AspNetCore.SignalR;

namespace TaskManagementApi.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendTaskUpdateNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}

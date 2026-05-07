using Microsoft.AspNetCore.SignalR;

namespace PropertyLeasing.API.Hubs
{
    public class MaintenanceHub : Hub
    {
        public async Task JoinMaintenanceBoard()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "MaintenanceBoard");
        }
    }
}

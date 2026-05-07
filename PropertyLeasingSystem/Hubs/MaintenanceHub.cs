using Microsoft.AspNetCore.SignalR;

namespace PropertyLeasingSystem.Hubs
{
    public class MaintenanceHub : Hub
    {
        public async Task JoinMaintenanceBoard()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "MaintenanceBoard");
        }
    }
}

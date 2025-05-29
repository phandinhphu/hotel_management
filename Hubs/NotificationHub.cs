using Microsoft.AspNetCore.SignalR;

namespace Hotel_Management.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;

            if (user != null && user.IsInRole("Admin"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
            }

            if (user != null && user.IsInRole("Staff"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Staff");
            }

            if (user != null && user.IsInRole("Customer"))
            {
                var userId = Context.UserIdentifier;
                Console.WriteLine($"User ID: {userId}"); // For debugging purposes
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Customer-{userId}");
            }

            await base.OnConnectedAsync();
        }
    }
}

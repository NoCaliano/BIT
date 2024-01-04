using Microsoft.AspNetCore.SignalR;

namespace BIT.Hubs
{
    public sealed class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task UpdateCart()
        {
            // я в потоці життя просто
            await Clients.All.SendAsync("CartUpdated");          
        }
    }
}

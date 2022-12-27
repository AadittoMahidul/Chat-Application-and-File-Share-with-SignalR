using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SignalRChatProject.Hubs
{
    public class ChatAndFileShareHub : Hub
    {
        private static Dictionary<string, string> users = new Dictionary<string, string>();
        private readonly IWebHostEnvironment env;
        public ChatAndFileShareHub(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("connected", "You are connected");
        }
        public async Task Join(string username)
        {
            users.Add(Context.ConnectionId, username);
            await Clients.All.SendAsync("userlist", users.Values.ToArray());
        }
        public async Task Send(string username, string message)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("message", username, message);
        }
        public async Task Upload(string username, FileData file)
        {
            string path = Path.Combine(this.env.WebRootPath, "Uploads");
            Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
            string data = regex.Replace(file.Data, string.Empty);
            byte[] buffer = Convert.FromBase64String(data);
            FileStream fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Create);
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();
            if (file.FileName.Contains(".jpg") || file.FileName.Contains(".png") || file.FileName.Contains(".gif"))
                await Clients.All.SendAsync("Uploaded", username, file.FileName, "image");
            else await Clients.All.SendAsync("Uploaded", username, file.FileName, "file");
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            users.Remove(Context.ConnectionId);
            Debug.Write(exception?.Message ?? string.Empty);
            return Task.CompletedTask;
        }
    }
    public class FileData
    {
        public string FileName { get; set; } = default!;
        public string Data { get; set; } = default!;

    }
}

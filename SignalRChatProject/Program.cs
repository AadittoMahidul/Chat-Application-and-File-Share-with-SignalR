using SignalRChatProject.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
var app = builder.Build();
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.MapHub<ChatAndFileShareHub>("/ChatAndFileShareHub");
app.Run();

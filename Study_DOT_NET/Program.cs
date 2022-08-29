using Study_DOT_NET.Models;
using Study_DOT_NET.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ChatDatabaseSettings>(
    builder.Configuration.GetSection("ChatDatabase"));
builder.Services.AddSingleton<MessagesService>();
builder.Services.AddSingleton<RoomsService>();
builder.Services.AddSingleton<UsersService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

using Study_DOT_NET.Shared.Models;
using Study_DOT_NET.Shared.Services;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Authentication.Cookies;
using Study_DOT_NET.Hubs;
using Study_DOT_NET.Shared.Builders;
using Microsoft.AspNetCore.Cors;
using Study_DOT_NET.Shared.Commands.Messages;
using Study_DOT_NET.Shared.Commands.Rooms;


var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("CorsPolicy", corsPolicyBuilder => corsPolicyBuilder
//         .WithOrigins("http://localhost:4200")
//         .AllowAnyMethod()
//         .AllowAnyHeader()
//         .AllowCredentials()
//         // .AllowAnyHeader()
//         // .AllowAnyMethod()
//         // .AllowAnyOrigin()
//     );
// });
builder.Services.AddCors();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/signin-github";
    options.LogoutPath = "/sign-out-github";
}).AddGitHub(options =>
{
    options.ClientId = builder.Configuration["Github:ClientId"];
    options.ClientSecret = builder.Configuration["Github:ClientSecret"];
});

builder.Services.AddSignalR(options => 
{ 
    options.EnableDetailedErrors = true; 
});

builder.Services.Configure<ChatDatabaseSettings>(
    builder.Configuration.GetSection("ChatDatabase"));

builder.Services.AddSingleton<MessagesService>();
builder.Services.AddSingleton<RoomsService>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<PrototypeRegistryService>();
builder.Services.AddSingleton<UserRegistryService>();

builder.Services.AddSingleton<CreateMessageCommand>();
builder.Services.AddSingleton<UpdateMessageCommand>();
builder.Services.AddSingleton<ReadMessageCommand>();
builder.Services.AddSingleton<DeleteMessageCommand>();

builder.Services.AddSingleton<CreateRoomCommand>();
builder.Services.AddSingleton<DeleteRoomCommand>();
builder.Services.AddSingleton<UpdateRoomCommand>();

builder.Services.AddControllers();

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(it =>
{
    it.AllowAnyHeader();
    it.AllowAnyMethod();
    it.AllowAnyOrigin();
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

/* *---* Map Hubs *---* */
app.MapHub<MessagesHub>("/message");
app.MapHub<RoomHub>("/room");
app.MapHub<UserHub>("/user");

app.Run();

/*
        <*----*>    List with todos     <*----*>


    ---> Send messages, rooms and invitations to CERTAIN users, not all:
            *>Some UserRegistryService, where i will contain a Key-Value pair storage, where:
                >Key is ConnectionId
                >Value is User._Id
            *>Suddenly it didn't work >:(
                
    ---> Authentication:
            *> TODO: google auth with OAuth2
            *> TODO: github auth with OAuth2
            *> TODO: simple registration [with an email confirmation]
 
        <*----*>    Extra todos    <*----*>
        
    ---> File sending
    ---> Encryption
 
 */
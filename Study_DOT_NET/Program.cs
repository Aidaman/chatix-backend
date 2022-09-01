using Study_DOT_NET.Shared.Models;
using Study_DOT_NET.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ChatDatabaseSettings>(
    builder.Configuration.GetSection("ChatDatabase"));
builder.Services.AddSingleton<MessagesService>();
builder.Services.AddSingleton<RoomsService>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<PrototypeRegistryService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseWebSockets();
app.UseAuthorization();
app.MapControllers();
app.Run();

/*
 * So i will write my ideas here:
 *
 * I would want to use Prototype design Pattern instead of Fabric. Prototype allow me to quickly configure a lot of objects,
 *  just by the copying one of them, instead of creating NEW one every single time it is needed
 *
 *
 * For encryption: I do not know for now how to implement this, i need to google it more
 *  still, i am completely sure that i will encrypt and decrypt messages in the client app, not on the backend
 *  But how should i implement that - is still a mystery
 *
 *
 * I would want to take some Advices on how it is better to transfer file across the app
 *  I think i will make this transfer in the binary format
 *  but HOW should i contain this one in the DataBase, and should i have a model for this in the backend
 *  (CRUD methods should be for file as well, and, if so, those would definitely need it's own controller and service)
 *
 *
 * As well, i would want to take advices about the structure of the app, how it's classes, and other functionality,
 *  better to implement; and which design patterns would be the best, but, for now:
 *
 *      For Socket Events i would want to implement Command Pattern
 *
 *      As for Chain of Responsibility - Socket Events can call it. For example: The client app initialization
 *          - it calls InitClientCommand, which means, that for this user backend shall:
 *          authorize the user -> pick available rooms -> count amount of unread messages -> send the result to the user
 *
 *      Mediator is what Service and DI propose, and Observer is the what WebSockets will be actually built,
 *          so i will use it automatically, without even notice
 *
 *      Strategy?
 *
 *      Also i would want to use Facade here, for, as an example, even more simplify and wrap Commands for Sockets
 *
 *
 * If OAuth (authorisation using Google/Github) is not a good solution for crypto-chat, then what would be better?
 * Registration and login using phone, or email?
 */
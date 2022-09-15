using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Study_DOT_NET.Models;

namespace Study_DOT_NET.Hubs
{
    public class MessageArgs
    {
        public Message _message;
        public string _roomid;
        public string _userId;

        public MessageArgs(Message message, string roomid, string userId)
        {
            this._message = message;
            this._roomid = roomid;
            this._userId = userId;
        }
    }

    public class MessagesHub : Hub
    {
        public async Task CreateMessage(string message)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nnew message: {message}\n");
                // Console.WriteLine(JsonSerializer.Serialize($"value: {(message, roomId, userId)}"));
                Console.ForegroundColor = ConsoleColor.White;


                await Clients.All.SendAsync("newMessage", message);
                // await Clients.Others.SendAsync("newMessage", JsonSerializer.Serialize($"value: {(message, roomId, userId)}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, message);
                throw;
            }

        }
    }
}

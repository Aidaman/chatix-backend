using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Commands.Messages;
using Study_DOT_NET.Shared.Commands.Rooms;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Hubs
{
    public class UserHub : Hub
    {
        private readonly UsersService _usersService;

        public UserHub(UsersService usersService)
        {
            this._usersService = usersService;
        }
        private async Task Connect(string userId)
        {
            if (userId is null)
            {
                throw new NullReferenceException("Can not connect empty user");
            }
            User user = await this._usersService.GetAsync(userId) 
                        ?? throw new NullReferenceException("There is no such user");
            user.IsOnline = true;
            await this._usersService.UpdateAsync(userId, user);
            
            await Clients.All.SendAsync("connected", user
                ?? throw new ApplicationException("room prototype, occasionally, is not the message object"));
        }

        private async Task Disconnect(string userId)
        {
            if (userId is null)
            {
                throw new NullReferenceException("Can not connect empty user");
            }
            User user = await this._usersService.GetAsync(userId) 
                        ?? throw new NullReferenceException("There is no such user");
            user.IsOnline = false;
            await this._usersService.UpdateAsync(userId, user);
            
            await Clients.All.SendAsync("disconnected", user
                ?? throw new ApplicationException("room prototype, occasionally, is not the message object"));
        }
    }
}
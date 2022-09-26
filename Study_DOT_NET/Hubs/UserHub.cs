using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Commands.Messages;
using Study_DOT_NET.Shared.Commands.Rooms;
using Study_DOT_NET.Shared.Commands.Users;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Hubs
{
    public class UserHub : Hub
    {
        private readonly ConnectCommand _connectCommand;
        private readonly DisconectCommand _disconectCommand;
        private readonly ChangeThemeCommand _changeThemeCommand;
        private readonly SearchUsersCommand _searchUsersCommand;

        public UserHub(PrototypeRegistryService prototypeRegistryService, RoomsService roomsService,
            UsersService usersService, MessagesService messagesService)
        {
            User? user = prototypeRegistryService.GetPrototypeById("message").Clone() as User;
            
            this._connectCommand = new ConnectCommand(user, new UserConfig(), usersService);
            this._disconectCommand = new DisconectCommand(user, new UserConfig(), usersService);
            this._changeThemeCommand = new ChangeThemeCommand(user, new UserConfig(), usersService);
            this._searchUsersCommand = new SearchUsersCommand(user, new UserConfig(), usersService);
        }

        private UserConfig GenerateUserConfig(List<string> user)
        {
            return JsonSerializer.Deserialize<UserConfig>(user[0]
                   ?? throw new NullReferenceException("room parameter is null"))
                   ?? throw new NullReferenceException("unsuccessful deserialization result is null");
        }

        private async Task Connect(List<string> user)
        {
            UserConfig config = this.GenerateUserConfig(user);
            this._connectCommand._userConfig = config;

            User? connectedUser = await this._connectCommand.Execute();

            await Clients.All.SendAsync("connected", JsonSerializer.Serialize<User>(connectedUser 
                                ?? throw new ApplicationException("room prototype, occasionally, is not the message object")));
        }
        private async Task Disconnect(List<string> user)
        {
            UserConfig config = this.GenerateUserConfig(user);
            this._disconectCommand._userConfig = config;

            User? disconnectedUser = await this._disconectCommand.Execute();

            await Clients.All.SendAsync("disconnected", JsonSerializer.Serialize<User>(disconnectedUser 
                                ?? throw new ApplicationException("room prototype, occasionally, is not the message object")));
        }

        private async Task SearchUsers(List<string> user)
        {
            UserConfig config = this.GenerateUserConfig(user);
            this._searchUsersCommand._userConfig = config;

            List<User>? users = await this._searchUsersCommand.Execute();

            await Clients.All.SendAsync("searchResult", JsonSerializer.Serialize<List<User>>(users
                                ?? throw new ApplicationException("room prototype, occasionally, is not the message object")));
        }
        private async Task ChangeTheme(List<string> user)
        {
            UserConfig config = this.GenerateUserConfig(user);
            this._changeThemeCommand._userConfig = config;
            await this._changeThemeCommand.Execute();
        }
    }
}

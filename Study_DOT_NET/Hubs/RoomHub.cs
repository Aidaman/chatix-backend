using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Commands.Messages;
using Study_DOT_NET.Shared.Commands.Rooms;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Hubs
{
    public class RoomHub : Hub
    {
        private readonly CreateRoomCommand _createRoomCommand;
        private readonly UpdateRoomCommand _updateRoomCommand;
        private readonly DeleteRoomCommand _deleteRoomCommand;
        private readonly CreateMessageCommand _createMessageCommand;
        private readonly UsersService _usersService;
        private readonly RoomsService _roomsService;
        public RoomHub(CreateRoomCommand createRoomCommand, UpdateRoomCommand updateRoomCommand, 
                       DeleteRoomCommand deleteRoomCommand, CreateMessageCommand createMessageCommand, 
                       UsersService usersService, RoomsService roomsService)
        {
            _createRoomCommand = createRoomCommand;
            _updateRoomCommand = updateRoomCommand;
            _deleteRoomCommand = deleteRoomCommand;
            _createMessageCommand = createMessageCommand;
            _usersService = usersService;
            _roomsService = roomsService;
        }
        private RoomConfig GenerateRoomConfig(List<string> room)
        {
            return JsonSerializer.Deserialize<RoomConfig>(room[0]
                   ?? throw new NullReferenceException("room parameter is null"))
                   ?? throw new NullReferenceException("unsuccessful deserialization result is null");
        }

        private async Task RoomDeleteOne(RoomConfig roomConfig)
        {
            try
            {
                this._deleteRoomCommand._roomConfig = roomConfig;
                Room? deletedRoom = await this._deleteRoomCommand.Execute();

                await Clients.All.SendAsync("roomDeleted", deletedRoom
                    ?? throw new ApplicationException("room prototype, occasionally, is not the message object"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private async Task UpdateRoom(RoomConfig roomConfig, string message, string eventMessage)
        {
            try
            {   
                this._updateRoomCommand._roomConfig = roomConfig;
                MessageConfig messageConfig = new MessageConfig()
                {
                    Id = Guid.NewGuid().ToString("N").Substring(0, 24),
                    IsForwarded = false,
                    IsSystem = true,
                    MessageContent = message,
                    RoomId = roomConfig.Id,
                    UserId = roomConfig.CreatorId,
                };
                this._createMessageCommand._messageConfig = messageConfig;

                Room? updatedRoom = await this._updateRoomCommand.Execute();
                Message? createdMessage = await this._createMessageCommand.Execute();

                Console.WriteLine(updatedRoom);
                
                await Clients.All.SendAsync(eventMessage, updatedRoom
                    ?? throw new ApplicationException("room prototype, occasionally, is not the room object"));

                await Clients.All.SendAsync("newMessage", createdMessage
                    ?? throw new ApplicationException("message prototype, occasionally, is not the message object"));

                Console.WriteLine($"room's amount of participants are: {updatedRoom.Participants.Count}");
                if (updatedRoom.ParticipantsIds.Count < 2)
                {
                    await this.RoomDeleteOne(roomConfig);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task CreateRoom(List<string> room)
        {
            try
            {
                RoomConfig roomConfig = GenerateRoomConfig(room);

                this._createRoomCommand._roomConfig = roomConfig;
                this._createRoomCommand._roomConfig!.Id = Guid.NewGuid().ToString("N").Substring(0, 24);

                MessageConfig messageConfig = new MessageConfig()
                {
                    Id = Guid.NewGuid().ToString("N").Substring(0, 24),
                    IsForwarded = false,
                    IsSystem = true,
                    MessageContent = $"Room has been created",
                    RoomId = roomConfig.Id,
                    UserId = roomConfig.CreatorId,
                };
                this._createMessageCommand._messageConfig = messageConfig;

                Room? createdRoom = await this._createRoomCommand.Execute();
                Message? createdMessage = await this._createMessageCommand.Execute();

                await Clients.All.SendAsync("newRoom", createdRoom
                    ?? throw new ApplicationException("room prototype, occasionally, is not the room object"));
                
                await Clients.All.SendAsync("newMessage", createdMessage 
                    ?? throw new ApplicationException("room prototype, occasionally, is not the message object"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task AddParticipant(List<string> room)
        {
            RoomConfig roomConfig = this.GenerateRoomConfig(room);
            roomConfig.IsAddUser = true;
            string message =
                $"{(await this._usersService.GetAsync(roomConfig.UserId))?.FullName} has joined the {(await this._roomsService.GetAsync(roomConfig.Id))?.Title}";
            await this.UpdateRoom(roomConfig, message, "userJoin");
        }
        public async Task PrivacyChange(List<string> room)
        {
            RoomConfig roomConfig = this.GenerateRoomConfig(room);
            roomConfig.IsAddUser = false;
            if (roomConfig.IsPublic != null)
            {
                await this.UpdateRoom(roomConfig, $"This room is now {((bool)roomConfig.IsPublic ? "public" : "private")}", "privacyChanged");   
            }
        }
        public async Task RenameRoom(List<string> room)
        {
            Console.WriteLine(room[0]);
            RoomConfig roomConfig = this.GenerateRoomConfig(room);
            roomConfig.IsAddUser = false;
            await this.UpdateRoom(roomConfig, $"Room has been renamed to: {roomConfig.Title}", "roomRename");
        }
        public async Task DeleteParticipant(List<string> room)
        {
            RoomConfig roomConfig = this.GenerateRoomConfig(room);
            roomConfig.IsAddUser = false;
            await this.UpdateRoom(roomConfig, $"{(await this._usersService.GetAsync(roomConfig.UserId)).FullName} left the room 😢", "userLeft");
        }

        public async Task DeleteRoom(List<string> room)
        {
            RoomConfig roomConfig = this.GenerateRoomConfig(room);
            await this.RoomDeleteOne(roomConfig);
        }
    }
}

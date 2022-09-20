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
        private readonly SearchRoomCommand _searchRoomCommand;
        private readonly CreateMessageCommand _createMessageCommand;

        private readonly UsersService _usersService;
        private readonly RoomsService _roomsService;
        private readonly MessagesService _messagesService;

        public RoomHub(PrototypeRegistryService prototypeRegistryService, RoomsService roomsService,
            UsersService usersService, MessagesService messagesService)
        {
            Room? room = prototypeRegistryService.GetPrototypeById("room") as Room;
            Message? message = prototypeRegistryService.GetPrototypeById("message") as Message;
            
            this._usersService = usersService;
            this._roomsService = roomsService;
            this._messagesService = messagesService;

            this._createRoomCommand = new CreateRoomCommand(room, new RoomConfig(), roomsService, usersService);
            this._updateRoomCommand = new UpdateRoomCommand(room, new RoomConfig(), roomsService, usersService);
            this._deleteRoomCommand = new DeleteRoomCommand(room, new RoomConfig(), roomsService);
            this._searchRoomCommand = new SearchRoomCommand(room, new RoomConfig(), roomsService);
            this._createMessageCommand = 
                new CreateMessageCommand(message, new MessageConfig(), this._messagesService,this._usersService);
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

                await Clients.All.SendAsync("roomDeleted", JsonSerializer.Serialize<Room>(deletedRoom
                    ?? throw new ApplicationException("room prototype, occasionally, is not the message object")));
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
                Console.WriteLine(roomConfig);
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

                await Clients.All.SendAsync(eventMessage, JsonSerializer.Serialize<Room>(updatedRoom
                    ?? throw new ApplicationException("room prototype, occasionally, is not the room object")));

                await Clients.All.SendAsync("newMessage", JsonSerializer.Serialize<Message>(createdMessage
                    ?? throw new ApplicationException("message prototype, occasionally, is not the message object")));

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

                await Clients.All.SendAsync("newRoom", JsonSerializer.Serialize<Room>(createdRoom
                    ?? throw new ApplicationException("room prototype, occasionally, is not the room object")));

                await Clients.All.SendAsync("newMessage", JsonSerializer.Serialize<Message>(createdMessage 
                    ?? throw new ApplicationException("room prototype, occasionally, is not the message object")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task PrivacyChange(List<string> room)
        {
            RoomConfig roomConfig = this.GenerateRoomConfig(room);
            await this.UpdateRoom(roomConfig, $"This room is now {(roomConfig.IsPublic ? "public" : "private")}", "privacyChanged");
        }
        public async Task RenameRoom(List<string> room)
        {
            RoomConfig roomConfig = this.GenerateRoomConfig(room);
            await this.UpdateRoom(roomConfig, $"Room has been renamed to: {roomConfig.Title}", "roomRename");
        }
        public async Task DeleteParticipant(List<string> room)
        {
            RoomConfig roomConfig = this.GenerateRoomConfig(room);
            await this.UpdateRoom(roomConfig, $"{(await this._usersService.GetAsync(roomConfig.UserId)).FullName} left the room 😢", "userLeft");
        }

        public async Task DeleteRoom(List<string> room)
        {
            RoomConfig roomConfig = this.GenerateRoomConfig(room);
            await this.RoomDeleteOne(roomConfig);
        }

        public async Task SearchRoom(List<string> room)
        {
            try
            {
                RoomConfig roomConfig = this.GenerateRoomConfig(room);

                List<Room>? searchedRooms = await this._searchRoomCommand.Execute();

                await Clients.All.SendAsync("newRoom", JsonSerializer.Serialize<List<Room>>(searchedRooms
                    ?? throw new ApplicationException("room prototype, occasionally, is not the message object")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task RoomInvitation(List<string> participant)
        {
            string newParticipant = JsonSerializer.Deserialize<string>(participant[0]) 
                                    ?? throw new NullReferenceException("participant parameter is null");

            
        }

        public async Task InvitationAccepted(List<string> participant)
        {
            string newParticipant = JsonSerializer.Deserialize<string>(participant[0]) 
                                    ?? throw new NullReferenceException("participant parameter is null");

            
        }
    }
}

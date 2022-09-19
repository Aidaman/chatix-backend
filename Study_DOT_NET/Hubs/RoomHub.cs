using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Study_DOT_NET.Models;
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

        private readonly UsersService _usersService;
        private readonly RoomsService _roomsService;

        public RoomHub(PrototypeRegistryService prototypeRegistryService, RoomsService roomsService,
            UsersService usersService)
        {
            Room? room = prototypeRegistryService.GetPrototypeById("room") as Room;

            this._usersService = usersService;
            this._roomsService = roomsService;

            this._createRoomCommand = new CreateRoomCommand(room, new RoomConfig(), roomsService);
            this._updateRoomCommand = new UpdateRoomCommand(room, new RoomConfig(), roomsService);
            this._deleteRoomCommand = new DeleteRoomCommand(room, new RoomConfig(), roomsService);
            this._searchRoomCommand = new SearchRoomCommand(room, new RoomConfig(), roomsService);
        }

        public async Task CreateRoom(List<string> room)
        {
            try
            {
                RoomConfig roomConfig = JsonSerializer.Deserialize<RoomConfig>(room[0] 
                    ?? throw new NullReferenceException("room parameter is null"))
                    ?? throw new NullReferenceException("unsuccessful deserialization result is null");

                this._createRoomCommand._roomConfig = roomConfig;
                this._createRoomCommand._roomConfig!.Id = Guid.NewGuid().ToString("N").Substring(0, 24);
                Room? createdRoom = await this._createRoomCommand.Execute();

                await Clients.All.SendAsync("newRoom", JsonSerializer.Serialize<Room>(createdRoom
                    ?? throw new ApplicationException("room prototype, occasionally, is not the message object")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task UpdateRoom(List<string> room)
        {
            try
            {
                RoomConfig config = JsonSerializer.Deserialize<RoomConfig>(room[0] 
                    ?? throw new NullReferenceException("room parameter is null"))
                    ?? throw new NullReferenceException("unsuccessful deserialization result is null");

                this._updateRoomCommand._roomConfig = config;
                Room? updatedRoom = await this._updateRoomCommand.Execute();

                await Clients.All.SendAsync("roomUpdated", JsonSerializer.Serialize<Room>(updatedRoom
                    ?? throw new ApplicationException("message prototype, occasionally, is not the message object")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task DeleteRoom(List<string> room)
        {
            try
            {
                RoomConfig config = JsonSerializer.Deserialize<RoomConfig>(room[0] 
                    ?? throw new NullReferenceException("room parameter is null"))
                    ?? throw new NullReferenceException("unsuccessful deserialization result is null");

                this._deleteRoomCommand._roomConfig = config;
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

        public async Task SearchRoom(List<string> room)
        {
            try
            {
                RoomConfig config = JsonSerializer.Deserialize<RoomConfig>(room[0] 
                    ?? throw new NullReferenceException("room parameter is null"))
                    ?? throw new NullReferenceException("unsuccessful deserialization result is null");

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

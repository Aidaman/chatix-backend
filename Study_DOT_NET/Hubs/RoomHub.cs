using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Commands.Messages;
using Study_DOT_NET.Shared.Commands.Rooms;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Hubs;

public class RoomHub : Hub
{
    private readonly CreateMessageCommand _createMessageCommand;
    private readonly CreateRoomCommand _createRoomCommand;
    private readonly DeleteRoomCommand _deleteRoomCommand;
    private readonly RoomsService _roomsService;
    private readonly UpdateRoomCommand _updateRoomCommand;
    private readonly UsersService _usersService;

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

    /// <summary>
    ///     This method generates new RoomConfig instance from json
    /// </summary>
    /// <param name="room">JSON representation of the room content that front-end gives here</param>
    /// <returns>new RoomConfig instance</returns>
    /// <exception cref="NullReferenceException">If instead of JSON null was given; or if deserialization was unsuccessful</exception>
    private RoomConfig GenerateRoomConfig(List<string> room)
    {
        return JsonSerializer.Deserialize<RoomConfig>(room[0]
                                                      ?? throw new NullReferenceException("room parameter is null"))
               ?? throw new NullReferenceException("unsuccessful deserialization result is null");
    }

    /// <summary>
    ///     Method that deletes room
    ///     Calls DeleteRoomCommand class
    /// </summary>
    /// <param name="roomConfig">Data about the room that should be deleted for DeleteRoomCommand class</param>
    /// <exception cref="ApplicationException"></exception>
    private async Task RoomDeleteOne(RoomConfig roomConfig)
    {
        try
        {
            _deleteRoomCommand._roomConfig = roomConfig;
            var deletedRoom = await _deleteRoomCommand.Execute();

            await Clients.All.SendAsync("roomDeleted", deletedRoom
                                                       ?? throw new ApplicationException(
                                                           "room prototype, occasionally, is not the message object"));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    /// <summary>
    ///     Method that Updates room
    ///     Calls UpdateRoomCommand class
    ///     Calls CreateMessageCommand class
    /// </summary>
    /// <param name="roomConfig">Data given by front-end about the room being updated</param>
    /// <param name="message">text of the message that generates for users to notice them, that room was updated</param>
    /// <param name="eventMessage">name of the event that should be called in the frontend</param>
    /// <exception cref="ApplicationException">If something went wrong while executing commands this exception thrown</exception>
    private async Task UpdateRoom(RoomConfig roomConfig, string message, string eventMessage)
    {
        try
        {
            _updateRoomCommand._roomConfig = roomConfig;
            var messageConfig = new MessageConfig
            {
                Id = Guid.NewGuid().ToString("N").Substring(0, 24),
                IsForwarded = false,
                IsSystem = true,
                MessageContent = message,
                RoomId = roomConfig.Id,
                UserId = roomConfig.CreatorId
            };
            _createMessageCommand._messageConfig = messageConfig;

            var updatedRoom = await _updateRoomCommand.Execute();
            var createdMessage = await _createMessageCommand.Execute();

            Console.WriteLine(updatedRoom);

            await Clients.All.SendAsync(eventMessage, updatedRoom
                                                      ?? throw new ApplicationException(
                                                          "room prototype, occasionally, is not the room object"));

            /*
             * If Room is Public: Do not invite users, but just add this room for them
             * If Room is Private: Do invite users, if they refuse - do not add them
             */

            await Clients.All.SendAsync("newMessage", createdMessage
                                                      ?? throw new ApplicationException(
                                                          "message prototype, occasionally, is not the message object"));

            // Console.WriteLine($"room's amount of participants are: {updatedRoom.Participants.Count}");
            if (updatedRoom.ParticipantsIds.Count < 2) await RoomDeleteOne(roomConfig);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    /// <summary>
    ///     Method that creates new Room
    ///     Calls CreateRoomCommand class
    ///     Calls CreateMessageCommand class
    /// </summary>
    /// <param name="room">JSON representation of RoomConfig class</param>
    /// <exception cref="ApplicationException">If something went wrong while executing commands this exception thrown</exception>
    public async Task CreateRoom(List<string> room)
    {
        try
        {
            var roomConfig = GenerateRoomConfig(room);

            _createRoomCommand._roomConfig = roomConfig;
            _createRoomCommand._roomConfig!.Id = Guid.NewGuid().ToString("N").Substring(0, 24);

            var messageConfig = new MessageConfig
            {
                Id = Guid.NewGuid().ToString("N").Substring(0, 24),
                IsForwarded = false,
                IsSystem = true,
                MessageContent = "Room has been created",
                RoomId = roomConfig.Id,
                UserId = roomConfig.CreatorId
            };
            _createMessageCommand._messageConfig = messageConfig;

            var createdRoom = await _createRoomCommand.Execute();
            var createdMessage = await _createMessageCommand.Execute();

            await Clients.All.SendAsync("newRoom", createdRoom
                                                   ?? throw new ApplicationException(
                                                       "room prototype, occasionally, is not the room object"));

            await Clients.All.SendAsync("newMessage", createdMessage
                                                      ?? throw new ApplicationException(
                                                          "room prototype, occasionally, is not the message object"));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    /// <summary>
    ///     A method that calls by front-end for updating a room
    /// </summary>
    /// <param name="room">JSON representation of RoomConfig class</param>
    public async Task AddParticipant(List<string> room)
    {
        var roomConfig = GenerateRoomConfig(room);
        roomConfig.IsAddUser = true;
        var message =
            $"{(await _usersService.GetAsync(roomConfig.UserId))?.FullName} has joined the {(await _roomsService.GetAsync(roomConfig.Id))?.Title}";
        await UpdateRoom(roomConfig, message, "userJoin");
    }

    /// <summary>
    ///     A method that calls by front-end for updating a room
    /// </summary>
    /// <param name="room">JSON representation of RoomConfig class</param>
    public async Task PrivacyChange(List<string> room)
    {
        var roomConfig = GenerateRoomConfig(room);
        roomConfig.IsAddUser = false;
        if (roomConfig.IsPublic != null)
            await UpdateRoom(roomConfig,
                $"This room is now {((bool)roomConfig.IsPublic ? "public" : "private")}", "privacyChanged");
    }

    /// <summary>
    ///     A method that calls by front-end for updating a room
    /// </summary>
    /// <param name="room">JSON representation of RoomConfig class</param>
    public async Task RenameRoom(List<string> room)
    {
        Console.WriteLine(room[0]);
        var roomConfig = GenerateRoomConfig(room);
        roomConfig.IsAddUser = false;
        await UpdateRoom(roomConfig, $"Room has been renamed to: {roomConfig.Title}", "roomRename");
    }

    /// <summary>
    ///     A method that calls by front-end for updating a room
    /// </summary>
    /// <param name="room">JSON representation of RoomConfig class</param>
    public async Task DeleteParticipant(List<string> room)
    {
        var roomConfig = GenerateRoomConfig(room);
        roomConfig.IsAddUser = false;
        await UpdateRoom(roomConfig,
            $"{(await _usersService.GetAsync(roomConfig.UserId)).FullName} left the room 😢", "userLeft");
    }

    /// <summary>
    ///     Method that deletes Room by calling RoomDeleteOne() method
    /// </summary>
    /// <param name="room">JSON representation of RoomConfig class</param>
    public async Task DeleteRoom(List<string> room)
    {
        var roomConfig = GenerateRoomConfig(room);
        await RoomDeleteOne(roomConfig);
    }

    public async Task InviteUsers(string[] participants, string roomId, string inviterId)
    {
        try
        {
            var room = await this._roomsService.GetAsync(roomId) 
                       ?? throw new NullReferenceException("There is no such room");
            var inviter = await this._usersService.GetAsync(inviterId) 
                          ?? throw new NullReferenceException("There is no such user");
        
            foreach (var invitee in participants)
            {
                Console.WriteLine(invitee);
                await Clients.All.SendAsync("inviteUser", inviter.FullName, invitee, room);   
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task AcceptInvitation(string roomId, string userId)
    {
        var user = await this._usersService.GetAsync(userId) ?? throw new NullReferenceException("There is no such user");
        var room = await this._roomsService.GetAsync(roomId) ?? throw new NullReferenceException("There is no such room");
        var roomConfig = new RoomConfig
        {
            IsAddUser = true,
            UserId = userId
        };
        await UpdateRoom(roomConfig, $"{user.FullName} accepted invitation to “{room.Title}”", "userJoined");
    }

    public async Task RejectInvitation(string roomId, string userId)
    {
        var user = await this._usersService.GetAsync(userId) ?? throw new NullReferenceException("There is no such user");
        var room = await this._roomsService.GetAsync(roomId) ?? throw new NullReferenceException("There is no such room");
        await UpdateRoom(new RoomConfig(), $"{user.FullName} rejected to join “{room.Title}”",
            "userRejectedInvitation");
    }
}
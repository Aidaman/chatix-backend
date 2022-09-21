using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Rooms;

public class CreateRoomCommand: RoomCommand
{
    private readonly UsersService _usersService;
    public CreateRoomCommand(Room room, RoomConfig data, RoomsService roomsService, UsersService usersService) 
        : base(room, data, roomsService)
    {
        this._usersService = usersService;
    }

    public override async Task<Room?> Execute()
    {
        if (this.prototype is Room room)
        {
            room.Id = this._roomConfig.Id;
            room.Title = this._roomConfig.Title;
            room.CreatorId = this._roomConfig.CreatorId;
            room.Creator = await this._usersService.GetAsync(this._roomConfig.CreatorId) ?? throw new NullReferenceException("There is no such User");
            room.LastAction = this._roomConfig.LastAction;
            room.ParticipantsIds = this._roomConfig.Participants;
            room.IsPublic = this._roomConfig.IsPublic;
            room.AmountOfUnread = 0;

            List<User?> participants = new List<User?>();
            foreach (string id in this._roomConfig.Participants)
            {
                participants.Add(await this._usersService.GetAsync(id));
            }
            room.Participants = participants;

            await this._roomsService.CreateAsync(room);
            return room;
        }
        else return null;
    }
}
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Rooms;

public class UpdateRoomCommand : RoomCommand
{
    private readonly UsersService _usersService;
    public UpdateRoomCommand(Room room, RoomConfig data, RoomsService roomsService, UsersService usersService)
        : base(room, data, roomsService)
    {
        this._usersService = usersService;
    }

    public override async Task<Room?> Execute()
    {
        if (this.prototype is Room room)
        {
            room = await this._roomsService.GetAsync(this._roomConfig.Id)
                   ?? throw new NullReferenceException("No such room is present in Database");

            room.Participants = new List<User>();

            if (this._roomConfig.IsPublic != null)
            {
                room.IsPublic = this._roomConfig.IsPublic;
            }

            room.ParticipantsIds = room.ParticipantsIds
                .Where((string participant) => participant != this._roomConfig.UserId).ToList();
            foreach (string participant in room.ParticipantsIds)
            {
                room.Participants.Add(await this._usersService.GetAsync(participant) ?? throw new NullReferenceException("There is no such User"));
            }

            // room.Participants = this._roomConfig.Participants;
            if (this._roomConfig.Title != String.Empty)
            {
                room.Title = this._roomConfig.Title;
            }

            await this._roomsService.UpdateAsync(room.Id, room);
            return room;
        }
        else return null;
    }
}
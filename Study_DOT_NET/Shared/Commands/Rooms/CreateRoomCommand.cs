using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Rooms;

public class CreateRoomCommand: RoomCommand
{
    public CreateRoomCommand(Room room, RoomConfig data, RoomsService roomsService) 
        : base(room, data, roomsService)
    {

    }

    public override async Task<Room?> Execute()
    {
        if (this.prototype is Room room)
        {
            room.Id = this._roomConfig.Id;
            room.Title = this._roomConfig.Title;
            room.CreatorId = this._roomConfig.CreatorId;
            room.LastAction = this._roomConfig.LastAction;
            room.Participants = this._roomConfig.Participants;
            room.IsPublic = this._roomConfig.IsPublic;
            room.AmountOfUnread = 0;

            await this._roomsService.CreateAsync(room);
            return room;
        }
        else return null;
    }
}
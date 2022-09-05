using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.Commands.Rooms;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.RoomAdministration;

public class UpdateRoomCommand : RoomCommand
{
    public UpdateRoomCommand(Room room, RoomConfig data, RoomsService roomsService)
        : base(room, data, roomsService)
    {

    }

    public override async Task Execute()
    {
        if (this.prototype is Room room)
        {
            room.Id = this._roomConfig.Id;
            room.Title = this._roomConfig.Title;
            room.CreatorId = this._roomConfig.CreatorId;
            room.LastAction = this._roomConfig.LastAction;
            room.Participants = this._roomConfig.Participants;

            await this._roomsService.UpdateAsync(room.Id, room);
        }
    }
}
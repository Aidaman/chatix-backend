using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.Commands.Rooms;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Rooms;

public class DeleteRoomCommand : RoomCommand
{
    public DeleteRoomCommand(Room room, RoomConfig data, RoomsService roomsService)
        : base(room, data, roomsService)
    {

    }

    public override async Task<Room?> Execute()
    {
        if (this.prototype is Room room)
        {
            await this._roomsService.RemoveAsync(this._roomConfig.Id);
            return await this._roomsService.GetAsync(this._roomConfig.Id);
        }
        else return null;
    }
}
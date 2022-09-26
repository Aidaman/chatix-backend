using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Rooms;

public class SearchRoomCommand : RoomCommand
{
    public SearchRoomCommand(Room room, RoomConfig data, RoomsService roomsService)
        : base(room, data, roomsService)
    {

    }

    public override async Task<List<Room>?> Execute()
    {
        if (this.prototype.Clone() is Room room)
        {
            return await this._roomsService.SearchAsync(this._roomConfig.Title, true);
        }
        else return null;
    }
}
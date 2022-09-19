using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Rooms;

public class SearchRoomCommand : RoomCommand
{
    private readonly RoomsService _roomsService;

    public SearchRoomCommand(Room room, RoomConfig data, RoomsService roomsService)
        : base(room, data, roomsService)
    {

    }

    public override async Task<List<Room>?> Execute()
    {
        if (this.prototype is Room room)
        {
            return await this._roomsService.SearchAsync(this._roomConfig.Title);
        }
        else return null;
    }
}
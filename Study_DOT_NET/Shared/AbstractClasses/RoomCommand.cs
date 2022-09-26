using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Abstract;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.AbstractClasses;

public abstract class RoomCommand: Command
{
    protected readonly RoomsService _roomsService;
    public RoomConfig _roomConfig;
    protected RoomCommand(Room room, RoomConfig data, RoomsService roomsService) : base(room)
    {
        _roomsService = roomsService;
        _roomConfig = data;
    }
}
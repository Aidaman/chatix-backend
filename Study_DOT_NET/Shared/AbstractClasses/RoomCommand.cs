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
        ((this.prototype as Room)!).Id = data.Id;
        ((this.prototype as Room)!).CreatorId = data.CreatorId;
        ((this.prototype as Room)!).LastAction = data.LastAction;
        ((this.prototype as Room)!).Participants = data.Participants;
        ((this.prototype as Room)!).Title = data.Title;

        _roomsService = roomsService;
        _roomConfig = data;
    }
}
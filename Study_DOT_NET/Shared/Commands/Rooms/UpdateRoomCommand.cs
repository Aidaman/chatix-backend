using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.RoomAdministration;

public class UpdateRoomCommand : Command
{
    private readonly RoomsService _roomsService;

    public UpdateRoomCommand(Room room, RoomConfig data, RoomsService roomsService) : base(room)
    {
        (this.prototype as Room)!.Id = data.Id;
        (this.prototype as Room)!.CreatorId = data.CreatorId;
        (this.prototype as Room)!.LastAction = data.LastAction;
        (this.prototype as Room)!.Participants = data.Participants;
        (this.prototype as Room)!.Title = data.Title;

        _roomsService = roomsService;
    }

    public override async Task Execute()
    {
        Room? room = this.prototype as Room;
        if (room != null)
        {
            await this._roomsService.UpdateAsync(room.Id, room);
        }
    }
}
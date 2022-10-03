using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.Commands.Rooms;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Rooms;

public class DeleteRoomCommand : RoomCommand
{
    public DeleteRoomCommand(PrototypeRegistryService prototypeRegistryService, RoomsService roomsService) 
        : base(prototypeRegistryService.GetPrototypeById("room") as Room, new RoomConfig(), roomsService)
    {

    }

    public override async Task<Room?> Execute()
    {
        if (this.prototype.Clone() is Room room)
        {
            Room deletedRoom = await this._roomsService.GetAsync(this._roomConfig.Id) ?? throw new NullReferenceException("There is no such Room");
            await this._roomsService.RemoveAsync(this._roomConfig.Id);
            return deletedRoom;
        }
        else return null;
    }
}
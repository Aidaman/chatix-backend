using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Rooms;

public class UpdateRoomCommand : RoomCommand
{

    public UpdateRoomCommand(Room room, RoomConfig data, RoomsService roomsService)
        : base(room, data, roomsService)
    {

    }

    public override async Task<Room?> Execute()
    {
        if (this.prototype is Room room)
        {
            room = await this._roomsService.GetAsync(this._roomConfig.Id) 
                   ?? throw new NullReferenceException("No such room is present in Database");

            room.IsPublic = this._roomConfig.IsPublic;
            foreach (string participant in room.Participants)
            {
                if (!this._roomConfig.Participants.Exists(p => p == participant))
                {
                    
                }
            }
            // room.Participants = this._roomConfig.Participants;
            room.Title = this._roomConfig.Title;

            await this._roomsService.UpdateAsync(room.Id, room);
            return room;
        }
        else return null;
    }
}
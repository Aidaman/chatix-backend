using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Rooms;

public class DeleteRoomCommand : RoomCommand
{
    private readonly UsersService _usersService;

    public DeleteRoomCommand(PrototypeRegistryService prototypeRegistryService, RoomsService roomsService,
        UsersService usersService)
        : base(prototypeRegistryService.GetPrototypeById("room") as Room, new RoomConfig(), roomsService)
    {
        _usersService = usersService;
    }

    public override async Task<Room?> Execute()
    {
        if (prototype.Clone() is Room room)
        {
            var deletedRoom = await _roomsService.GetAsync(_roomConfig.Id) ??
                              throw new NullReferenceException("There is no such Room");
            await _roomsService.RemoveAsync(_roomConfig.Id);

            foreach (var participant in room.Participants)
            {
                participant.RoomIds.RemoveAll(x => x == room.Id);
                await _usersService.UpdateAsync(participant._Id, participant);
            }

            return deletedRoom;
        }

        return null;
    }
}
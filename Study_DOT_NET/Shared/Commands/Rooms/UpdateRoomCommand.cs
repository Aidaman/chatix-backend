using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Rooms;

public class UpdateRoomCommand : RoomCommand
{
    private readonly UsersService _usersService;

    public UpdateRoomCommand(PrototypeRegistryService prototypeRegistryService, RoomsService roomsService,
        UsersService usersService)
        : base(prototypeRegistryService.GetPrototypeById("room") as Room, new RoomConfig(), roomsService)
    {
        _usersService = usersService;
    }

    public override async Task<Room?> Execute()
    {
        if (prototype.Clone() is Room room)
        {
            room = await _roomsService.GetAsync(_roomConfig.Id)
                   ?? throw new NullReferenceException("No such room is present in Database");

            room.Participants = new List<User>();
            room.Creator = await _usersService.GetAsync(room.CreatorId);

            if (_roomConfig.IsPublic != null) room.IsPublic = _roomConfig.IsPublic;

            if (_roomConfig.Title != string.Empty) room.Title = _roomConfig.Title;

            if (_roomConfig.IsAddUser &&
                room.CreatorId != _roomConfig.UserId &&
                !room.ParticipantsIds.Contains(_roomConfig.UserId))
            {
                room.ParticipantsIds.Add(_roomConfig.UserId);
            }
            else
            {
                if (_roomConfig.UserId == room.CreatorId)
                    throw new ApplicationException("Creator can not delete itself");

                if (_roomConfig.UserId != string.Empty)
                    room.ParticipantsIds = room.ParticipantsIds
                        .Where(participant => participant != _roomConfig.UserId)
                        .ToList();
            }

            foreach (var participant in room.ParticipantsIds)
                room.Participants.Add(await _usersService.GetAsync(participant) ??
                                      throw new NullReferenceException("There is no such User"));

            await _roomsService.UpdateAsync(room.Id, room);
            return room;
        }

        return null;
    }
}
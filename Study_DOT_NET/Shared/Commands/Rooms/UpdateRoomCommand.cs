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
        this._usersService = usersService;
    }

    public override async Task<Room?> Execute()
    {
        if (this.prototype.Clone() is Room room)
        {
            room = await this._roomsService.GetAsync(this._roomConfig.Id)
                   ?? throw new NullReferenceException("No such room is present in Database");

            room.Participants = new List<User>();
            room.Creator = await this._usersService.GetAsync(room.CreatorId);

            if (this._roomConfig.IsPublic != null)
            {
                room.IsPublic = this._roomConfig.IsPublic;
            }

            if (this._roomConfig.Title != String.Empty)
            {
                room.Title = this._roomConfig.Title;
            }

            if (this._roomConfig.IsAddUser)
            {
                room.ParticipantsIds.Add(this._roomConfig.UserId);
            }
            else
            {
                if (_roomConfig.UserId == room.CreatorId)
                {
                    throw new ApplicationException("Creator can not delete itself");
                }

                if (_roomConfig.UserId != String.Empty)
                {
                    room.ParticipantsIds = room.ParticipantsIds
                        .Where((string participant) => participant != this._roomConfig.UserId)
                        .ToList();
                }
            }

            foreach (string participant in room.ParticipantsIds)
            {
                room.Participants.Add(await this._usersService.GetAsync(participant) ??
                                      throw new NullReferenceException("There is no such User"));
            }

            await this._roomsService.UpdateAsync(room.Id, room);
            return room;
        }
        else return null;
    }
}
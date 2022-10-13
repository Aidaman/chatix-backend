using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.Builders;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Rooms;

public class CreateRoomCommand: RoomCommand
{
    /* <*---*> ConcurrentDictionary? <*---*> */
    private readonly UsersService _usersService;
    public CreateRoomCommand(PrototypeRegistryService prototypeRegistryService, RoomsService roomsService, UsersService usersService) 
        : base(prototypeRegistryService.GetPrototypeById("room") as Room, new RoomConfig(), roomsService)
    {
        this._usersService = usersService;
    }

    public override async Task<Room?> Execute()
    {
        if (this.prototype.Clone() is Room room)
        {
            RoomBuilder builder = new RoomBuilder(room, this._roomsService, this._usersService, null);
            builder.ConfigureBasicParameters(this._roomConfig, null);
            await builder.ConfigureParticipants();
            await builder.ConfigureCreator();
            room = builder.Build() as Room ?? throw new ApplicationException("Builder can not build the room properly");

            room.Creator = await this._usersService.GetAsync(this._roomConfig.CreatorId) ?? throw new NullReferenceException("There is no such User");
            room.AmountOfUnread = 0;

            await this._roomsService.CreateAsync(room);

            foreach (User participant in room.Participants)
            {
                if (!participant.RoomIds.Any(x => x == room.Id))
                {
                    participant.RoomIds.Add(room.Id);
                    await this._usersService.UpdateAsync(participant._Id, participant);
                }
            }
            
            return room;
        }
        else return null;
    }
}
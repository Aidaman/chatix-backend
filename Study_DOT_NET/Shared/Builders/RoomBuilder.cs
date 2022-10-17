using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Interfaces;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Builders;

public class RoomBuilder : IChatBuilder
{
    private readonly MessagesService? _messagesService;
    private readonly RoomsService _roomsService;
    private readonly UsersService _usersService;
    private readonly IPrototype room;

    public RoomBuilder(Room room, RoomsService roomsService, UsersService usersService,
        MessagesService? messagesService)
    {
        _roomsService = roomsService;
        _usersService = usersService;
        _messagesService = messagesService;
        this.room = room.Clone();
    }

    public IPrototype Build()
    {
        return room;
    }

    public RoomBuilder ConfigureBasicParameters(string id, string creatorId, string title, DateTime lastAction,
        bool? isFavorites, bool? isPublic)
    {
        (room as Room)!.Id = id;
        (room as Room)!.CreatorId = creatorId;
        (room as Room)!.Title = title;
        (room as Room)!.LastAction = lastAction;
        (room as Room)!.IsFavorites = isFavorites ?? false;
        (room as Room)!.IsPublic = isPublic ?? false;
        return this;
    }

    public RoomBuilder ConfigureBasicParameters(RoomConfig config, bool? isFavorites)
    {
        (room as Room)!.Id = config.Id;
        (room as Room)!.CreatorId = config.CreatorId;
        (room as Room)!.Title = config.Title;
        (room as Room)!.LastAction = config.LastAction;
        (room as Room)!.IsPublic = config.IsPublic;
        (room as Room)!.IsFavorites = isFavorites ?? false;
        (room as Room)!.ParticipantsIds = config.Participants;
        return this;
    }

    public async Task<RoomBuilder> ConfigureCreator()
    {
        (room as Room).Creator = await _usersService.GetAsync((room as Room)!.CreatorId)
                                 ?? throw new NullReferenceException("There iis no such User");
        return this;
    }

    public async Task<RoomBuilder> ConfigureParticipants()
    {
        (room as Room)!.Participants = new List<User>();

        foreach (var id in (room as Room)!.ParticipantsIds)
            (room as Room)!.Participants.Add(await _usersService.GetAsync(id)
                                             ?? throw new NullReferenceException("There is no such User"));

        return this;
    }

    public async Task<RoomBuilder> ConfigureUnread(string userId)
    {
        if (_messagesService != null)
        {
            var messages =
                await _messagesService!.GetRoomContentAsync((room as Room)!.Id, 0, 100)
                ?? throw new ApplicationException(
                    $"Can not get messages for room \"{(room as Room)!.Title}\"");
            (room as Room)!.AmountOfUnread =
                messages.Where(x => !x.ReadBy.Contains(userId)
                                    && x.CreatorId != userId &&
                                    !x.IsSystemMessage).ToList().Count;
        }
        else
        {
            throw new NullReferenceException("Message Service for room builder is null");
        }

        return this;
    }
}
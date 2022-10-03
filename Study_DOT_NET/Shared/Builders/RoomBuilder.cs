using MongoDB.Bson.Serialization.Serializers;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Interfaces;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Builders
{
    public class RoomBuilder : IChatBuilder
    {
        private readonly RoomsService _roomsService;
        private readonly UsersService _usersService;
        private readonly MessagesService? _messagesService;
        private readonly IPrototype room;

        public RoomBuilder(Room room, RoomsService roomsService, UsersService usersService,
            MessagesService? messagesService)
        {
            this._roomsService = roomsService;
            this._usersService = usersService;
            this._messagesService = messagesService;
            this.room = room.Clone();
        }

        public RoomBuilder ConfigureBasicParameters(string id, string creatorId, string title, DateTime lastAction,
            bool? isFavorites, bool? isPublic)
        {
            (this.room as Room)!.Id = id;
            (this.room as Room)!.CreatorId = creatorId;
            (this.room as Room)!.Title = title;
            (this.room as Room)!.LastAction = lastAction;
            (this.room as Room)!.IsFavorites = isFavorites ?? false;
            (this.room as Room)!.IsPublic = isPublic ?? false;
            return this;
        }

        public RoomBuilder ConfigureBasicParameters(RoomConfig config, bool? isFavorites)
        {
            (this.room as Room)!.Id = config.Id;
            (this.room as Room)!.CreatorId = config.CreatorId;
            (this.room as Room)!.Title = config.Title;
            (this.room as Room)!.LastAction = config.LastAction;
            (this.room as Room)!.IsPublic = config.IsPublic;
            (this.room as Room)!.IsFavorites = isFavorites ?? false;
            (this.room as Room)!.ParticipantsIds = config.Participants;
            return this;
        }

        public async Task<RoomBuilder> ConfigureCreator()
        {
            (this.room as Room).Creator = await this._usersService.GetAsync((this.room as Room)!.CreatorId)
                                          ?? throw new NullReferenceException("There iis no such User");
            return this;
        }

        public async Task<RoomBuilder> ConfigureParticipants()
        {
            (this.room as Room)!.Participants = new List<User>();

            foreach (string id in (this.room as Room)!.ParticipantsIds)
            {
                (this.room as Room)!.Participants.Add(await this._usersService.GetAsync(id)
                                                      ?? throw new NullReferenceException("There is no such User"));
            }

            return this;
        }

        public async Task<RoomBuilder> ConfigureUnread(string userId)
        {
            if (this._messagesService != null)
            {
                List<Message> messages =
                    await this._messagesService!.GetRoomContentAsync((this.room as Room)!.Id, 0, 100)
                    ?? throw new ApplicationException(
                        $"Can not get messages for room \"{(this.room as Room)!.Title}\"");
                (this.room as Room)!.AmountOfUnread = 
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

        public IPrototype Build()
        {
            return this.room as IPrototype;
        }
    }
}
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Models;

namespace Study_DOT_NET.Shared.Services
{
    public class RoomsService
    {
        private readonly IMongoCollection<Room> _roomsCollection;
        private readonly UsersService _usersService;
        public List<User> Participants { get; set; } = new List<User>();

        public RoomsService()
        {

        }
        public RoomsService(IOptions<ChatDatabaseSettings> chatDatabaseSettings, UsersService usersService)
        {
            MongoClient mongoClient = new MongoClient(chatDatabaseSettings.Value.ConnectionString);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(chatDatabaseSettings.Value.DatabaseName);
            this._roomsCollection = mongoDatabase.GetCollection<Room>(chatDatabaseSettings.Value.RoomsCollectionName);
            this._usersService = usersService;
        }

        public async Task<List<Room>> GetAsync() =>
            await this._roomsCollection.Find(_ => true).ToListAsync();

        public async Task<Room?> GetAsync(string id) =>
            await this._roomsCollection.Find((Room x) => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Room>> GetAvailableRoomsAsync(string userId) =>
            await this._roomsCollection
                .Find((Room x) => x.ParticipantsIds.Contains(userId))
                .ToListAsync();

        public async Task CreateAsync(Room newRoom) =>
            await this._roomsCollection.InsertOneAsync(newRoom);

        public async Task UpdateAsync(string id, Room updatedRoom) =>
            await this._roomsCollection.ReplaceOneAsync((Room x) => x.Id == id, updatedRoom);

        public async Task RemoveAsync(string id) =>
            await this._roomsCollection.DeleteOneAsync((Room x) => x.Id == id);
        public async Task<List<Room>> SearchAsync(string title, bool isPublic) =>
            await this._roomsCollection.Find((Room x) => x.Title.Contains(title) && x.IsPublic == isPublic).ToListAsync();
    }
}

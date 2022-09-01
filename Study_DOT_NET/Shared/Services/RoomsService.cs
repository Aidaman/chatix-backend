using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Models;

namespace Study_DOT_NET.Shared.Services
{
    public class RoomsService
    {
        private readonly IMongoCollection<Room> _roomsCollection;

        public RoomsService(IOptions<ChatDatabaseSettings> chatDatabaseSettings)
        {
            MongoClient mongoClient = new MongoClient(chatDatabaseSettings.Value.ConnectionString);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(chatDatabaseSettings.Value.DatabaseName);
            this._roomsCollection = mongoDatabase.GetCollection<Room>(chatDatabaseSettings.Value.RoomsCollectionName);
        }

        public async Task<List<Room>> GetAsync() =>
            await this._roomsCollection.Find(_ => true).ToListAsync();

        public async Task<Room?> GetAsync(string id) =>
            await this._roomsCollection.Find((Room x) => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Room newRoom) =>
            await this._roomsCollection.InsertOneAsync(newRoom);

        public async Task UpdateAsync(string id, Room updatedRoom) =>
            await this._roomsCollection.ReplaceOneAsync((Room x) => x.Id == id, updatedRoom);

        public async Task RemoveAsync(string id) =>
            await this._roomsCollection.DeleteOneAsync((Room x) => x.Id == id);
        public async Task<List<Room>> SearchAsync(string title) =>
            await this._roomsCollection.Find((Room x) => x.Title.Contains(title)).ToListAsync();
    }
}

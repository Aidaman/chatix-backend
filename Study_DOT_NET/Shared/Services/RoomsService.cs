using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Models;

namespace Study_DOT_NET.Shared.Services;

public class RoomsService
{
    private readonly IMongoCollection<Room> _roomsCollection;
    private readonly UsersService _usersService;

    public RoomsService()
    {
    }

    public RoomsService(IOptions<ChatDatabaseSettings> chatDatabaseSettings, UsersService usersService)
    {
        var mongoClient = new MongoClient(chatDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(chatDatabaseSettings.Value.DatabaseName);
        _roomsCollection = mongoDatabase.GetCollection<Room>(chatDatabaseSettings.Value.RoomsCollectionName);
        _usersService = usersService;
    }

    public List<User> Participants { get; set; } = new();

    public async Task<List<Room>> GetAsync()
    {
        return await _roomsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Room?> GetAsync(string id)
    {
        return await _roomsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Room>> GetAvailableRoomsAsync(string userId, bool isPublic)
    {
        return await _roomsCollection
            .Find(x => x.ParticipantsIds.Contains(userId) && x.IsPublic == isPublic)
            .ToListAsync();
    }

    public async Task CreateAsync(Room newRoom)
    {
        await _roomsCollection.InsertOneAsync(newRoom);
    }

    public async Task UpdateAsync(string id, Room updatedRoom)
    {
        await _roomsCollection.ReplaceOneAsync(x => x.Id == id, updatedRoom);
    }

    public async Task RemoveAsync(string id)
    {
        await _roomsCollection.DeleteOneAsync(x => x.Id == id);
    }

    public async Task<List<Room>> SearchAsync(string title)
    {
        return await _roomsCollection
            .Find(x => x.Title.ToLower().Contains(title.ToLower()) && x.IsPublic == true)
            .ToListAsync();
    }
}
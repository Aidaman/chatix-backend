using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Models;

namespace Study_DOT_NET.Shared.Services;

public class UsersService
{
    private readonly User _currentUser = null!;
    //TODO: Authentication

    private readonly IMongoCollection<User> _usersCollection;

    public UsersService(IOptions<ChatDatabaseSettings> chatDatabaseSettings)
    {
        var mongoClient = new MongoClient(chatDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(chatDatabaseSettings.Value.DatabaseName);
        _usersCollection = mongoDatabase.GetCollection<User>(chatDatabaseSettings.Value.UsersCollectionName);
    }

    public async Task<List<User>> GetAsync()
    {
        return await _usersCollection.Find(_ => true).ToListAsync();
    }

    public async Task<User?> GetAsync(string id)
    {
        return await _usersCollection.Find(x => (BsonString)x._Id == (BsonString)id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(User newUser)
    {
        await _usersCollection.InsertOneAsync(newUser);
    }

    public async Task UpdateAsync(string id, User updatedUser)
    {
        await _usersCollection.ReplaceOneAsync(x => x._Id == id, updatedUser);
    }

    public async Task RemoveAsync(string id)
    {
        await _usersCollection.DeleteOneAsync(x => x._Id == id);
    }

    public async Task<List<User>> SearchAsync(string name)
    {
        if (name != "*")
            return await _usersCollection.Find(x => x.FullName.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        return await GetAsync();
    }
}
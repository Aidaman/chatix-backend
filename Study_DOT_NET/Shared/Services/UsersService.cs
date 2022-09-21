using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Models;

namespace Study_DOT_NET.Shared.Services;

public class UsersService
{
    private readonly User _currentUser = null!;
    //TODO: Authentication

    private readonly IMongoCollection<User> _usersCollection;

    public UsersService()
    {

    }
    public UsersService(IOptions<ChatDatabaseSettings> chatDatabaseSettings)
    {
        MongoClient mongoClient = new MongoClient(chatDatabaseSettings.Value.ConnectionString);
        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(chatDatabaseSettings.Value.DatabaseName);
        this._usersCollection = mongoDatabase.GetCollection<User>(chatDatabaseSettings.Value.UsersCollectionName);
    }

    public async Task<List<User>> GetAsync() =>
        await this._usersCollection.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string id) => 
        await this._usersCollection.Find((User x) => x._Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser) =>
        await this._usersCollection.InsertOneAsync(newUser);

    public async Task UpdateAsync(string id, User updatedUser) =>
        await this._usersCollection.ReplaceOneAsync((User x) => x._Id == id, updatedUser);

    public async Task RemoveAsync(string id) =>
        await this._usersCollection.DeleteOneAsync((User x) => x._Id == id);
    public async Task<List<User>> SearchAsync(string name) =>
        await this._usersCollection.Find((User x) => x.FullName.Contains(name)).ToListAsync();
}
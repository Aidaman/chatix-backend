using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Models;

namespace Study_DOT_NET.Shared.Services;

public class MessagesService
{
    private readonly IMongoCollection<Message> _messagesCollection;
    
    public MessagesService(IOptions<ChatDatabaseSettings> chatDatabaseSettings)
    {
        MongoClient mongoClient = new MongoClient(chatDatabaseSettings.Value.ConnectionString);
        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(chatDatabaseSettings.Value.DatabaseName);
        this._messagesCollection = mongoDatabase.GetCollection<Message>(chatDatabaseSettings.Value.MessagesCollectionName);
    }
    
    public async Task<List<Message>> GetAsync() => 
        await this._messagesCollection.Find(_ => true).ToListAsync();

    public async Task<Message?> GetAsync(string id) =>
        await this._messagesCollection.Find((Message x) => x.Id == id).FirstOrDefaultAsync();

    public async Task<List<Message>?> GetRoomContentAsync(string roomId, int offset, int limit)  => 
        await this._messagesCollection
            .Find((Message x) => x.RoomId == roomId)
            .SortByDescending((Message message) => message.CreatedAt)
            .Skip(offset)
            .Limit(limit)
            .ToListAsync();

    public async Task<List<Message>?> getAllMessagesFromRoom(string roomId) =>
        await this._messagesCollection
            .Find((Message x) => x.RoomId == roomId)
            .ToListAsync();

    public async Task CreateAsync(Message newMessage) =>
        await this._messagesCollection.InsertOneAsync(newMessage);

    public async Task UpdateAsync(string id, Message updatedMessage) =>
        await this._messagesCollection.ReplaceOneAsync((Message x) => x.Id == id, updatedMessage);

    public async Task RemoveAsync(string id) =>
        await this._messagesCollection.DeleteOneAsync((Message x) => x.Id == id);
    public async Task<List<Message>> SearchAsync(string content) =>
        await this._messagesCollection.Find((Message x) => x.MessageContent.Contains(content)).ToListAsync();
}
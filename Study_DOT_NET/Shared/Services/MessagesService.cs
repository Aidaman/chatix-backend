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
        var mongoClient = new MongoClient(chatDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(chatDatabaseSettings.Value.DatabaseName);
        _messagesCollection =
            mongoDatabase.GetCollection<Message>(chatDatabaseSettings.Value.MessagesCollectionName);
    }

    public async Task<List<Message>> GetAsync()
    {
        return await _messagesCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Message?> GetAsync(string id)
    {
        return await _messagesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Message>?> GetRoomContentAsync(string roomId, int offset, int limit)
    {
        return await _messagesCollection
            .Find(x => x.RoomId == roomId)
            .SortByDescending(message => message.CreatedAt)
            .Skip(offset)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<List<Message>?> getAllMessagesFromRoom(string roomId)
    {
        return await _messagesCollection
            .Find(x => x.RoomId == roomId)
            .ToListAsync();
    }

    public async Task CreateAsync(Message newMessage)
    {
        await _messagesCollection.InsertOneAsync(newMessage);
    }

    public async Task UpdateAsync(string id, Message updatedMessage)
    {
        await _messagesCollection.ReplaceOneAsync(x => x.Id == id, updatedMessage);
    }

    public async Task RemoveAsync(string id)
    {
        await _messagesCollection.DeleteOneAsync(x => x.Id == id);
    }

    public async Task<List<Message>> SearchAsync(string content)
    {
        return await _messagesCollection.Find(x => x.MessageContent.Contains(content)).ToListAsync();
    }
}
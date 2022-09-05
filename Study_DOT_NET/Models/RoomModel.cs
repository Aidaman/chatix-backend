using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Study_DOT_NET.Shared.Interfaces;

namespace Study_DOT_NET.Models;

public class Room: IPrototype
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("title")]
    public string Title { get; set; } = null!;

    [BsonElement("lastAction")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime LastAction { get; set; } = DateTime.Now;

    [BsonElement("creator")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CreatorId { get; set; } = null!;

    [BsonElement("users")]
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> Participants { get; set; } = null!;

    [BsonElement("isFavorites")] 
    public bool IsFavorites { get; set; } = false;

    [BsonElement("isPublic")] 
    public bool IsPublic { get; set; } = false;

    [BsonElement("unread")] 
    public Int32 AmountOfUnread { get; set; } = 0;

    [BsonElement("__v")]
    public Int32 __V { get; set; }

    public IPrototype Clone()
    {
        return (IPrototype)MemberwiseClone();
    }
}
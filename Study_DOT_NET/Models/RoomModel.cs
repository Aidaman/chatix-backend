using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Study_DOT_NET.Models;

public class Room
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = null;

    [BsonElement("title")]
    public string? Title { get; set; } = null;

    [BsonElement("lastAction")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime? LastAction { get; set; } = null;

    [BsonElement("creator")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? CreatorId { get; set; } = null;

    [BsonElement("users")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string[]? Participants { get; set; } = null;

    [BsonElement("isFavorites")]
    public bool IsFavorites { get; set; }

    [BsonElement("isPublic")]
    public bool IsPublic { get; set; }

    [BsonElement("unread")]
    public Int32 AmountOfUnread { get; set; }

    [BsonElement("__v")]
    public Int32 __V { get; set; }
}
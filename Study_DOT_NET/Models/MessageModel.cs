using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Study_DOT_NET.Models;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = null;

    [BsonElement("content")] 
    public string? MessageContent { get; set; } = null;

    [BsonElement("read")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string[]? ReadBy { get; set; } = null;

    [BsonElement("createdAt")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime? CreatedAt { get; set; } = null;

    [BsonElement("updatedAt")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime? UpdatedAt { get; set; } = null;

    [BsonElement("creator")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? CreatorId { get; set; } = null;

    [BsonElement("room")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? RoomId { get; set; } = null;

    [BsonElement("isSystemMessage")]
    public bool IsSystemMessage { get; set; }

    [BsonElement("isForwardedMessage")]
    public bool IsForwardedMessage { get; set; }

    [BsonElement("__v")]
    public Int32 __V { get; set; }
}
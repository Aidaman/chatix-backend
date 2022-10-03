using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Study_DOT_NET.Shared.Interfaces;
using System.Text.Json.Serialization;

namespace Study_DOT_NET.Models;

public class Room: IPrototype
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("_id")]
    public string Id { get; set; } = null!;

    [BsonElement("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [BsonElement("lastAction")]
    [BsonRepresentation(BsonType.DateTime)]
    [JsonPropertyName("lastAction")]
    public DateTime LastAction { get; set; } = DateTime.Now;

    [BsonElement("creator")]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore]
    public string CreatorId { get; set; } = null!;

    [BsonIgnore]
    [JsonPropertyName("creator")]
    public User? Creator { get; set; } = null!;

    [BsonElement("users")]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore]
    public List<string> ParticipantsIds { get; set; } = null!;

    [BsonIgnore]
    [JsonPropertyName("users")]
    public List<User> Participants { get; set; } = null!;

    [BsonElement("isFavorites")]
    [JsonPropertyName("isFavorites")]
    public bool IsFavorites { get; set; } = false;

    [BsonElement("isPublic")]
    [JsonPropertyName("isPublic")]
    public bool? IsPublic { get; set; } = null;

    [BsonElement("unread")]
    [JsonPropertyName("unread")]
    public Int32 AmountOfUnread { get; set; } = 0;

    [BsonElement("__v")]
    [JsonIgnore]
    public Int32 __V { get; set; }

    public IPrototype Clone()
    {
        return (IPrototype)MemberwiseClone();
    }
}
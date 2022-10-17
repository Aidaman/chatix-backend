using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Study_DOT_NET.Shared.Interfaces;

namespace Study_DOT_NET.Models;

public class User : IPrototype
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("_id")]
    public string _Id { get; set; } = null!;

    [BsonElement("socketIds")]
    [JsonIgnore]
    // [BsonRepresentation(BsonType.Array)]
    public List<string> SocketIds { get; set; } = null!;

    [BsonElement("isPremium")]
    [JsonPropertyName("isPremium")]
    public bool IsPremium { get; set; } = false;

    [BsonElement("isOnline")]
    [JsonPropertyName("isOnline")]
    public bool IsOnline { get; set; } = false;

    [BsonElement("blacklist")] public List<string> Blacklist { get; set; } = null!;

    [BsonElement("colorTheme")] public string ColorTheme { get; set; } = null!;

    [BsonElement("id")] public string? Id { get; set; } = null;

    [BsonElement("name")]
    [JsonPropertyName("name")]
    public string FullName { get; set; } = null!;

    [BsonElement("avatar")]
    [JsonPropertyName("avatar")]
    public string Avatar { get; set; } = null!;

    [BsonElement("roomIds")]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore]
    public List<string> RoomIds { get; set; } = null!;

    [BsonElement("__v")] [JsonIgnore] public int __V { get; set; }

    public IPrototype Clone()
    {
        return (IPrototype)MemberwiseClone();
    }
}
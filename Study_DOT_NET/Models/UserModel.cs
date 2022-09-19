using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Study_DOT_NET.Shared.Interfaces;

namespace Study_DOT_NET.Models;

public class User: IPrototype
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("_id")]
    public string _Id { get; set; } = null!;

    [BsonElement("socketIds")]
    // [BsonRepresentation(BsonType.Array)]
    public List<string> SocketIds { get; set; } = null!;

    [BsonElement("isPremium")] 
    public bool IsPremium { get; set; } = false;

    [BsonElement("isOnline")] 
    public bool IsOnline { get; set; } = false;

    [BsonElement("blacklist")]
    public List<string> Blacklist { get; set; } = null!;

    [BsonElement("colorTheme")]
    public string ColorTheme { get; set; } = null!;

    [BsonElement("id")]
    public string? Id { get; set; } = null;

    [BsonElement("name")] 
    public string FullName { get; set; } = null!;

    [BsonElement("avatar")] 
    public string Avatar { get; set; } = null!;

    [BsonElement("__v")]
    [JsonPropertyName("__v")]
    public Int32 __V { get; set; }

    public IPrototype Clone()
    {
        return (IPrototype)MemberwiseClone();
    }
}
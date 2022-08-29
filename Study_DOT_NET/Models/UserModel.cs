using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Study_DOT_NET.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("_id")]
    public string? _Id { get; set; } = null;

    [BsonElement("socketIds")]
    // [BsonRepresentation(BsonType.Array)]
    public string[]? SocketIds { get; set; } = null;

    [BsonElement("isPremium")]
    public bool IsPremium { get; set; }

    [BsonElement("isOnline")]
    public bool IsOnline { get; set; }

    [BsonElement("blacklist")]
    public string[]? Blacklist { get; set; } = null;

    [BsonElement("colorTheme")]
    public string? ColorTheme { get; set; } = null;

    [BsonElement("id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = null;

    [BsonElement("name")]
    public string FullName { get; set; }

    [BsonElement("avatar")]
    public string Avatar { get; set; }

    [BsonElement("__v")]
    [JsonPropertyName("__v")]
    public Int32 __V { get; set; }
}
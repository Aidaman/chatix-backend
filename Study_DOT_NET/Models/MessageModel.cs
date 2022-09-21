using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Study_DOT_NET.Shared.Interfaces;

namespace Study_DOT_NET.Models;

public class Message : IPrototype
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("_id")]
    public string Id { get; set; } = null!;

    [BsonElement("content")]
    [JsonPropertyName("content")]
    public string MessageContent { get; set; } = null!;

    [BsonElement("read")]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("read")]
    public List<string> ReadBy { get; set; } = null!;

    [BsonElement("createdAt")]
    [BsonRepresentation(BsonType.DateTime)]
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; } = null;
    
    [BsonElement("updatedAt")]
    [BsonRepresentation(BsonType.DateTime)]
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; } = null;

    [BsonElement("creator")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CreatorId { get; set; } = null!;

    [BsonIgnore]
    [JsonPropertyName("creator")]
    public User? Creator { get; set; }

    [BsonElement("room")]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("room")]
    public string RoomId { get; set; } = null!;

    [BsonElement("isSystemMessage")]
    [JsonPropertyName("isSystemMessage")]
    public bool IsSystemMessage { get; set; } = false;

    [BsonElement("isForwardedMessage")]
    [JsonPropertyName("isForwardedMessage")]
    public bool IsForwardedMessage { get; set; } = false;

    [BsonElement("__v")]
    [JsonIgnore]
    public Int32 __v { get; set; }

    public IPrototype Clone()
    {
        return (IPrototype)MemberwiseClone();
    }

    public override string ToString()
    {
        return $"\n>{this.CreatorId} ({this.Creator.FullName}), {this.CreatedAt}<\n-->{this.MessageContent}\n {(this.IsForwardedMessage? "Forwarded, " : "")}{(this.IsSystemMessage ? "System, " : "")}from {this.RoomId}\n";
    }
}
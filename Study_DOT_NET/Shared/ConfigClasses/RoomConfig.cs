using System.Text.Json.Serialization;

namespace Study_DOT_NET.Shared.ConfigClasses;

public class RoomConfig
{
    [JsonPropertyName("roomId")] public string Id { get; set; } = string.Empty;
    [JsonPropertyName("creatorId")] public string CreatorId { get; set; } = string.Empty;
    [JsonPropertyName("userId")] public string UserId { get; set; } = string.Empty;
    [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;
    [JsonPropertyName("participants")] public List<string> Participants { get; set; } = null!;

    [JsonPropertyName("lastAction")] public DateTime LastAction { get; set; } = DateTime.Now;

    [JsonPropertyName("isPublic")] public bool? IsPublic { get; set; } = null;
    [JsonIgnore] public bool IsAddUser { get; set; }
}
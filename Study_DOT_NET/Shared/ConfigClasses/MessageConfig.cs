using System.Text.Json.Serialization;

namespace Study_DOT_NET.Shared.ConfigClasses;

public class MessageConfig
{
    [JsonPropertyName("messageId")] public string Id { get; set; } = string.Empty;

    [JsonPropertyName("content")] public string MessageContent { get; set; } = string.Empty;

    /*
     *  >User Id description<
     *  This field describes ID of any user that can be passed as the argument to an event
     *  For example, when it is "CreateMessage" then it is the creator of message
     *  Or if it is "ReadMessage" then it is ID of User who read the message
     */
    [JsonPropertyName("userId")] public string? UserId { get; set; } = string.Empty;
    [JsonPropertyName("roomId")] public string RoomId { get; set; } = string.Empty;
    [JsonPropertyName("isForwarded")] public bool IsForwarded { get; set; } = false;
    [JsonPropertyName("IsSystem")] public bool IsSystem { get; set; } = false;

    public override string ToString()
    {
        return $"{UserId} in {RoomId} wrote:\n {Id} -> {MessageContent}";
    }
}
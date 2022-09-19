using System.Text.Json.Serialization;

namespace Study_DOT_NET.Shared.ConfigClasses
{
    public class MessageConfig
    {
        [JsonPropertyName("messageId")]
        public string Id { get; set; } = String.Empty;
        [JsonPropertyName("newContent")]
        public string MessageContent { get; set; } = String.Empty;
        /*
         *  >User Id description<
         *  This field describes ID of any user that can be passed as the argument to an event
         *  For example, when it is "CreateMessage" then it is the creator of message
         *  Or if it is "ReadMessage" then it is ID of User who read the message
         */
        [JsonPropertyName("userId")]
        public string UserId { get; set; } = String.Empty;
        [JsonPropertyName("roomId")]
        public string RoomId { get; set; } = String.Empty;
        [JsonPropertyName("IsForwarded")]
        public bool IsForwarded { get; set; } = false;
        [JsonPropertyName("IsSystem")]
        public bool IsSystem { get; set; } = false;

        public override string ToString()
        {
            return $"{this.UserId} in {this.RoomId} wrote:\n {this.Id} -> {this.MessageContent}";
        }
    }
}

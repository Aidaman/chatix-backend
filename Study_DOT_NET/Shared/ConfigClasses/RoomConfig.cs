using System.Text.Json.Serialization;

namespace Study_DOT_NET.Shared.ConfigClasses
{
    public class RoomConfig
    {
        [JsonPropertyName("roomId")]
        public string Id { get; set; } = String.Empty;
        [JsonPropertyName("creatorId")]
        public string CreatorId { get; set; } = String.Empty;
        [JsonPropertyName("userId")]
        public string UserId { get; set; } = String.Empty;
        [JsonPropertyName("title")]
        public string Title { get; set; } = String.Empty;
        [JsonPropertyName("participants")]
        public List<string> Participants { get; set; } = null!;

        [JsonPropertyName("lastAction")]
        public DateTime LastAction { get; set; } = DateTime.Now;

        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }
        [JsonIgnore]
        public bool IsAddUser { get; set; }

        public override string ToString()
        {
            return $"{this.Id ?? "this"} is\na {(this.IsPublic ? "Public" : "Private")} room, named {this.Title ?? "somehow"}\n created by: {this.CreatorId ?? "unknown user"}\n -> last action date {this.LastAction}\n -> {(this.Participants != null ? this.Participants.Count : "with unknown amount of users")} participants";
        }
    }
}

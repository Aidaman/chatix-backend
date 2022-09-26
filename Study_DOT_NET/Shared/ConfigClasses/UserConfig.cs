using System.Text.Json.Serialization;

namespace Study_DOT_NET.Shared.ConfigClasses
{
    public class UserConfig
    {
        [JsonPropertyName("userId")]
        public string Id { get; set; } = String.Empty;
        [JsonPropertyName("name")]
        public string Name { get; set; } = String.Empty;

    }
}

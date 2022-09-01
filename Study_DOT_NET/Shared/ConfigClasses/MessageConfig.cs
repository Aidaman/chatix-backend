namespace Study_DOT_NET.Shared.ConfigClasses
{
    public class MessageConfig
    {
        public string Id { get; set; } = String.Empty;
        public string MessageContent { get; set; } = String.Empty;
        public string CreatorId { get; set; } = String.Empty;
        public bool IsForwarded { get; set; } = false;
    }
}

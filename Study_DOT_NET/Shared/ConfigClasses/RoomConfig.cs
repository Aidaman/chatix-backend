namespace Study_DOT_NET.Shared.ConfigClasses
{
    public class RoomConfig
    {
        public string Id { get; set; } = String.Empty;
        public string CreatorId { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
        public List<string> Participants { get; set; } = null!;
        public DateTime LastAction { get; set; } = DateTime.Now;
    }
}

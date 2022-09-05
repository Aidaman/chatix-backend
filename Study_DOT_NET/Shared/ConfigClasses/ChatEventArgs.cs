namespace Study_DOT_NET.Shared.ConfigClasses
{
    public class ChatEventArgs: EventArgs
    {
        public string Command { get; }
        public object Args { get; }
        public string messageId { get; }
        public string roomId { get; }
        public string userId { get; }
    }
}

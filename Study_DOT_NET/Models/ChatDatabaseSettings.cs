namespace Study_DOT_NET.Models;

public class ChatDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string MessagesCollectionName { get; set; } = null!;
    public string RoomsCollectionName { get; set; } = null!;
    public string UsersCollectionName { get; set; } = null!;
}
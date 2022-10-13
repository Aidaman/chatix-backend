namespace Study_DOT_NET.Shared.Services;

public class UserRegistryService
{
    /// <summary>
    /// Keys are User's Ids, Values are Connection's Ids
    /// </summary>
    private Dictionary<string, string> UserRegistry { get; }

    public UserRegistryService()
    {
        UserRegistry = new Dictionary<string, string>();
    }

    /// <summary>
    ///  Gets a connectionId from a dictionary
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>string representation of connection Id, or null if there were not such connection id</returns>
    public string? GetConnectionId(string userId)
    {
        try
        {
            return this.UserRegistry[userId];
        }
        catch (Exception e)
        {
            return null;
        }
    }

    /// <summary>
    ///  Gets a UserId from a dictionary
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns>string representation of user Id, or null if there were not such user id</returns>
    public string? GetUserId(string connectionId)
    {
        return this.UserRegistry.Values.FirstOrDefault(x => x == connectionId, null);
    }

    /// <summary>
    ///  Adds new record to a dictionary
    /// </summary>
    /// <param name="connectionId"> Connection id that tied to a user's id </param>
    /// <param name="userId"> User id is a generic user's id </param>
    /// <returns>generic string that contains user id and connection id one after another</returns>
    public string AppendOne(string connectionId, string userId)
    {
        Console.WriteLine($"connectionId: {connectionId} | userId: {userId}");
        if (this.UserRegistry.ContainsKey(userId))
        {
            this.UserRegistry[userId] = connectionId;
        }
        else
        {
            this.UserRegistry.Add(userId, connectionId);
        }
        return $"{userId} : {connectionId}";
    }

    /// <summary>
    ///  Removes a record from a dictionary
    /// </summary>
    /// <param name="userId"> User id is a generic user's id </param>
    /// <returns>Generic string that says id that were removed</returns>
    public string RemoveOne(string userId)
    {
        this.UserRegistry.Remove(userId);
        return $"Removed {userId}";
    }

    public override string ToString()
    {
        string res = "";
        foreach (KeyValuePair<string, string> pair in UserRegistry)
        {
            Console.WriteLine($"*> Pair: {pair}");
            res += $"--> {pair.Key} : {pair.Value}\n";
        }

        return res;
    }
}
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Interfaces;

namespace Study_DOT_NET.Shared.Services;

public class PrototypeRegistryService
{
    private readonly Dictionary<string, IPrototype> Prototypes = new();

    public PrototypeRegistryService()
    {
        AddNewPrototype("message", new Message());
        AddNewPrototype("room", new Room());
        AddNewPrototype("user", new User());

        // this.AddNewPrototype("file", new ChatFile());???
    }

    private void AddNewPrototype(string id, IPrototype prototype)
    {
        Prototypes.Add(id, prototype);
    }

    public IPrototype GetPrototypeById(string id)
    {
        return Prototypes[id].Clone();
    }
}
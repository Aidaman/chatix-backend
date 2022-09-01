using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Interfaces;

namespace Study_DOT_NET.Shared.Services
{
    public class PrototypeRegistryService
    {
        private Dictionary<string, IPrototype> Prototypes = new Dictionary<string, IPrototype>();

        public PrototypeRegistryService()
        {
            this.AddNewPrototype("message", new Message());
            this.AddNewPrototype("room", new Room());
            this.AddNewPrototype("user", new User());

            // this.AddNewPrototype("file", new ChatFile());???
        }

        private void AddNewPrototype(string id, IPrototype prototype)
        {
            this.Prototypes.Add(id, prototype);
        }

        public IPrototype GetPrototypeById(string id)
        {
            return this.Prototypes[id].Clone();
        }
    }
}

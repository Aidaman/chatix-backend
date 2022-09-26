using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Interfaces;

namespace Study_DOT_NET.Shared.Abstract;

public abstract class Command
{
    public IPrototype prototype;

    public Command(IPrototype prototype)
    {
        this.prototype = prototype;
    }

    public abstract Task Execute();
}
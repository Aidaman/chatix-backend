namespace Study_DOT_NET.Shared.Interfaces;

public interface IPrototype
{
    IPrototype Clone();
}

/*
 *  TODO: Prototypes registry and PrototypeService, to be able to give consumer clones that needed
 */
namespace Study_DOT_NET.Shared.Services;

public class SocketService
{
    private readonly MessagesService _messagesService;
    private readonly RoomsService _roomsService;
    private readonly UsersService _usersService;
    private readonly PrototypeRegistryService _prototypeRegistryService;

    public SocketService(MessagesService messagesService, RoomsService roomsService, UsersService usersService, PrototypeRegistryService prototypeRegistryService)
    {
        _messagesService = messagesService;
        _roomsService = roomsService;
        _usersService = usersService;
        _prototypeRegistryService = prototypeRegistryService;
    }


}
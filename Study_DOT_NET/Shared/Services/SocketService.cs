using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Commands.Messages;
using Study_DOT_NET.Shared.Commands.RoomAdministration;
using Study_DOT_NET.Shared.Commands.Rooms;
using Study_DOT_NET.Shared.Commands.Users;
using Study_DOT_NET.Shared.ConfigClasses;

namespace Study_DOT_NET.Shared.Services;

public class SocketService
{
    // private readonly PrototypeRegistryService _prototypeRegistryService;
    // private readonly MessagesService _messagesService;
    // private readonly RoomsService _roomsService;
    private readonly UsersService _usersService;

    private readonly CreateMessageCommand _createMessageCommand;
    private readonly DeleteMessageCommand _deleteMessageCommand;
    private readonly UpdateMessageCommand _updateMessageCommand;
    private readonly ReadMessageCommand _readMessageCommand;

    private readonly CreateRoomCommand _createRoomCommand;
    private readonly DeleteRoomCommand _deleteRoomCommand;
    private readonly SearchRoomCommand _searchRoomCommand;
    private readonly UpdateRoomCommand _updateRoomCommand;

    private readonly ChangeThemeCommand _changeThemeCommand;
    private readonly ConnectCommand _connectCommand;
    private readonly DisconectCommand _disconectCommand;
    private readonly SearchUsersCommand _searchUsersCommand;

    public SocketService(PrototypeRegistryService prototypeRegistryService, 
                         MessagesService messagesService, 
                         RoomsService roomService, 
                         UsersService usersService)
    {
        // _prototypeRegistryService = prototypeRegistryService;
        // _messagesService = messagesService;
        // _roomsService = roomService;
        _usersService = usersService;

        Message? message = prototypeRegistryService.GetPrototypeById("message") as Message;
        Room? room = prototypeRegistryService.GetPrototypeById("room") as Room;
        User? user = prototypeRegistryService.GetPrototypeById("user") as User;

        this._createMessageCommand = new CreateMessageCommand(message!, new MessageConfig(), messagesService);
        this._updateMessageCommand = new UpdateMessageCommand(message!, new MessageConfig(), messagesService);
        this._readMessageCommand = new ReadMessageCommand(message!, new MessageConfig(), messagesService, "");
        this._deleteMessageCommand = new DeleteMessageCommand(message!, new MessageConfig(), messagesService);

        this._createRoomCommand = new CreateRoomCommand(room!, new RoomConfig(), roomService);
        this._updateRoomCommand = new UpdateRoomCommand(room!, new RoomConfig(), roomService);
        this._searchRoomCommand = new SearchRoomCommand(room!, new RoomConfig(), roomService);
        this._deleteRoomCommand = new DeleteRoomCommand(room!, new RoomConfig(), roomService);

        this._connectCommand = new ConnectCommand(user!, new UserConfig(), usersService);
        this._disconectCommand = new DisconectCommand(user!, new UserConfig(), usersService);
        this._searchUsersCommand = new SearchUsersCommand(user!, new UserConfig(), usersService);
        this._changeThemeCommand = new ChangeThemeCommand(user!, new UserConfig(), usersService);
    }

    private static MessageConfig generateMessageConfig(Message message)
    {
        return new MessageConfig()
        {
            CreatorId = message.CreatorId,
            Id = message.Id,
            IsForwarded = message.IsForwardedMessage,
            MessageContent = message.MessageContent,
        };
    }
    private static RoomConfig generateRoomConfig(Room room)
    {
        return new RoomConfig()
        {
            Id = room.Id,
            CreatorId = room.CreatorId,
            LastAction = room.LastAction,
            Participants = room.Participants,
            Title = room.Title,
        };
    }
    private static UserConfig generateUserConfig(User user)
    {
        return new UserConfig()
        {
            Id = user._Id,
            Name = user.FullName,
        };
    }

    /* *---* Area responsible for Messages Events *---* */
    public async Task CreateMessageAsync(ChatEventArgs args)
    {
        if (args.Args is Message message)
        {
            this._createMessageCommand._messageConfig = generateMessageConfig(message);
            await this._createMessageCommand.Execute();
        }
    }
    public async Task DeleteMessageAsync(ChatEventArgs args)
    {
        if (args.Args is Message message)
        {
            this._deleteMessageCommand._messageConfig = generateMessageConfig(message);
            await this._deleteMessageCommand.Execute();
        }
    }
    public async Task ReadMessageAsync(ChatEventArgs args)
    {
        if (args.Args is Message message)
        {
            this._readMessageCommand._messageConfig = generateMessageConfig(message);
            await this._readMessageCommand.Execute();
        }
    }
    public async Task UpdateMessageAsync(ChatEventArgs args)
    {
        if (args.Args is Message message)
        {
            this._updateMessageCommand._messageConfig = generateMessageConfig(message);
            await this._updateMessageCommand.Execute();
        }
    }

    /* *---* Area responsible for Room Events *---* */
    public async Task CreateRoomAsync(ChatEventArgs args)
    {
        if (args.Args is Room room)
        {
            this._createRoomCommand._roomConfig = generateRoomConfig(room);
            await this._createRoomCommand.Execute();
        }
    }
    public async Task DeleteRoomAsync(ChatEventArgs args)
    {
        if (args.Args is Room room)
        {
            this._deleteRoomCommand._roomConfig = generateRoomConfig(room);
            await this._deleteRoomCommand.Execute();
        }
    }
    public async Task SearchRoomAsync(ChatEventArgs args)
    {
        if (args.Args is Room room)
        {
            this._searchRoomCommand._roomConfig = generateRoomConfig(room);
            await this._searchRoomCommand.Execute();
        }
    }
    public async Task UpdateRoomAsync(ChatEventArgs args)
    {
        if (args.Args is Room room)
        {
            this._updateRoomCommand._roomConfig = generateRoomConfig(room);
            await this._updateRoomCommand.Execute();

            if (args.Command == "userJoined")
            {
                User? user = await this._usersService.GetAsync(args.userId);
                this._createMessageCommand._messageConfig = new MessageConfig()
                {
                    MessageContent = $"{user?.FullName} has joined this room 😀",
                    CreatorId = "system",
                    Id = "ID", //GENERATE UNIQUE ID
                    // IsForwarded = args.Args.isForwarded;
                };
                await this._createMessageCommand.Execute();
            }
        }
    }

    /* *---* Area responsible for User Events *---* */
    public async Task ChangeThemeAsync(ChatEventArgs args)
    {
        if (args.Args is User user)
        {
            this._changeThemeCommand._userConfig = generateUserConfig(user);
            await this._changeThemeCommand.Execute();
        }
    }
    public async Task ConnectUserAsync(ChatEventArgs args)
    {
        if (args.Args is User user)
        {
            this._changeThemeCommand._userConfig = generateUserConfig(user);
            await this._connectCommand.Execute();
        }
    }
    public async Task DisconnectUserAsync(ChatEventArgs args)
    {
        if (args.Args is User user)
        {
            this._disconectCommand._userConfig = generateUserConfig(user);
            await this._disconectCommand.Execute();
        }
    }
    public async Task SearchUsersAsync(ChatEventArgs args)
    {
        if (args.Args is User user)
        {
            this._searchUsersCommand._userConfig = generateUserConfig(user);
            await this._searchUsersCommand.Execute();
        }
    }
}
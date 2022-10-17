using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages;

public class ReadMessageCommand : MessageCommand
{
    private readonly RoomsService _roomsService;
    private readonly UsersService _usersService;

    public ReadMessageCommand(PrototypeRegistryService prototypeRegistryService, MessagesService messagesService,
        UsersService usersService, RoomsService roomsService)
        : base(prototypeRegistryService.GetPrototypeById("message") as Message, new MessageConfig(),
            messagesService)
    {
        _roomsService = roomsService;
        _usersService = usersService;
    }

    public override async Task<Message?> Execute()
    {
        if (prototype.Clone() is Message message && _messageConfig.UserId != null)
        {
            message = await _messagesService.GetAsync(_messageConfig.Id) ??
                      throw new NullReferenceException("There is no such message");
            message.Creator = await _usersService.GetAsync(message.CreatorId);
            if (message.CreatorId != _messageConfig.UserId &&
                !message.ReadBy.Contains(_messageConfig.UserId))
            {
                message.ReadBy.Add(_messageConfig.UserId);
                await _messagesService.UpdateAsync(message.Id, message);
                return message;
            }

            return null;
        }

        return null;
    }
}
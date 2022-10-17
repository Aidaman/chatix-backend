using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages;

public class UpdateMessageCommand : MessageCommand
{
    private readonly UsersService _usersService;

    public UpdateMessageCommand(PrototypeRegistryService prototypeRegistryService, MessagesService messagesService,
        UsersService usersService)
        : base(prototypeRegistryService.GetPrototypeById("message") as Message, new MessageConfig(), messagesService)
    {
        _usersService = usersService;
    }

    public override async Task<Message?> Execute()
    {
        if (prototype.Clone() is Message message)
        {
            message = await _messagesService.GetAsync(_messageConfig.Id) ??
                      throw new NullReferenceException("Such message does not exist in the DB");
            message.MessageContent = _messageConfig.MessageContent;

            await _messagesService.UpdateAsync(message.Id, message);
            message.Creator = await _usersService.GetAsync(message.CreatorId);

            return message;
        }

        return null;
    }
}
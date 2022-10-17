using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages;

public class CreateMessageCommand : MessageCommand
{
    private readonly UsersService _usersService;

    public CreateMessageCommand(PrototypeRegistryService prototypeRegistryService, MessagesService messagesService,
        UsersService usersService)
        : base(prototypeRegistryService.GetPrototypeById("message") as Message, new MessageConfig(), messagesService)
    {
        _usersService = usersService;
    }

    private async Task BuildMessage(Message message)
    {
        message.MessageContent = _messageConfig.MessageContent;
        message.IsSystemMessage = _messageConfig.IsSystem;
        message.RoomId = _messageConfig.RoomId;
        message.CreatorId = _messageConfig.UserId;
        message.CreatedAt = DateTime.Now;
        message.UpdatedAt = DateTime.Now;
    }

    public override async Task<Message?> Execute()
    {
        if (prototype.Clone() is Message message)
        {
            await BuildMessage(message);

            if (_messageConfig.IsForwarded)
            {
                message = await _messagesService.GetAsync(_messageConfig.Id) ??
                          throw new NullReferenceException("There is no such message");

                message.RoomId = _messageConfig.RoomId;
                message.CreatedAt = DateTime.Now;
                message.UpdatedAt = DateTime.Now;

                message.IsForwardedMessage = true;
                message.Id = Guid.NewGuid().ToString("N").Substring(0, 24);
            }

            message.ReadBy = new List<string>();
            message.Creator = await _usersService.GetAsync(message.CreatorId);

            await _messagesService.CreateAsync(message);
            return message;
        }

        return null;
    }
}
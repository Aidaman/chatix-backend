using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages;

public class DeleteMessageCommand : MessageCommand
{
    public DeleteMessageCommand(PrototypeRegistryService prototypeRegistryService, MessagesService messagesService)
        : base(prototypeRegistryService.GetPrototypeById("message") as Message, new MessageConfig(),
            messagesService)
    {
    }

    public override async Task<Message?> Execute()
    {
        if (prototype.Clone() is Message message)
        {
            message = await _messagesService.GetAsync(_messageConfig.Id) ??
                      throw new NullReferenceException("There is no such Message");
            await _messagesService.RemoveAsync(message.Id);
            return message;
        }

        return null;
    }
}
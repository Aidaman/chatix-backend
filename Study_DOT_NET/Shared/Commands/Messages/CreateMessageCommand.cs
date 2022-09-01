using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages;

public class CreateMessageCommand: Command
{
    private readonly MessagesService _messagesService;
    public CreateMessageCommand(Message message, MessageConfig data, MessagesService messagesService): base(message)
    {
        ((this.prototype as Message)!).Id = data.Id;
        ((this.prototype as Message)!).MessageContent = data.MessageContent;
        ((this.prototype as Message)!).CreatorId = data.CreatorId;
        ((this.prototype as Message)!).IsForwardedMessage = data.IsForwarded;

        if (data.CreatorId == "system" || data.CreatorId == String.Empty)
        {
            ((this.prototype as Message)!).IsSystemMessage = true;
        }

        this._messagesService = messagesService;
    }

    public override async Task Execute()
    {
        Message? message = this.prototype as Message;
        if (message != null)
        {
            await this._messagesService.CreateAsync(message);
        }
    }
}
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages;

public class CreateMessageCommand: MessageCommand
{
    public CreateMessageCommand(Message message, MessageConfig data, MessagesService messagesService) 
        : base(message, data, messagesService)
    {

    }

    public override async Task Execute()
    {
        if (this.prototype is Message message)
        {
            message.Id = this._messageConfig.Id;
            message.CreatorId = this._messageConfig.CreatorId;
            message.MessageContent = this._messageConfig.MessageContent;
            message.IsForwardedMessage = this._messageConfig.IsForwarded;
            message.CreatedAt = DateTime.Now;

            await this._messagesService.CreateAsync(message);
        }
    }
}
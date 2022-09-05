using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages;

public class UpdateMessageCommand : MessageCommand
{
    private readonly MessagesService _messagesService;
    public UpdateMessageCommand(Message message, MessageConfig data, MessagesService messagesService)
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

            await this._messagesService.UpdateAsync(message.Id, message);
        }
    }
}
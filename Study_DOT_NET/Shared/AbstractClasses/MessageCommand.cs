using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Abstract;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.AbstractClasses;

public abstract class MessageCommand: Command
{
    protected readonly MessagesService _messagesService;
    public MessageConfig _messageConfig;

    protected MessageCommand(Message message, MessageConfig data, MessagesService messagesService) : base(message)
    {
        (this.prototype as Message)!.Id = data.Id;
        (this.prototype as Message)!.MessageContent = data.MessageContent;
        (this.prototype as Message)!.CreatorId = data.CreatorId;
        (this.prototype as Message)!.IsForwardedMessage = data.IsForwarded;

        if (data.CreatorId == "system" || data.CreatorId == String.Empty)
        {
            ((this.prototype as Message)!).IsSystemMessage = true;
        }

        this._messagesService = messagesService;
        _messageConfig = data;
    }
}
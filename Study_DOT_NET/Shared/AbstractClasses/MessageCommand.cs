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
        if (data.UserId == "system" || data.UserId == String.Empty)
        {
            ((this.prototype as Message)!).IsSystemMessage = true;
        }

        this._messagesService = messagesService;
        _messageConfig = data;
    }
}
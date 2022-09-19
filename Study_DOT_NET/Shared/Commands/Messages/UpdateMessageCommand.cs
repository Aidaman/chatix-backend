using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages;

public class UpdateMessageCommand : MessageCommand
{
    public UpdateMessageCommand(Message message, MessageConfig data, MessagesService messagesService)
        : base(message, data, messagesService)
    {

    }

    public override async Task<Message?> Execute()
    {
        if (this.prototype is Message message)
        {
            message = await this._messagesService.GetAsync(this._messageConfig.Id) ??
                      throw new NullReferenceException("Such message does not exist in the DB");
            message.MessageContent = this._messageConfig.MessageContent;

            await this._messagesService.UpdateAsync(message.Id, message);
            return message;
        }
        else return null;
    }
}
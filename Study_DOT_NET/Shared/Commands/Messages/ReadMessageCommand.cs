using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Interfaces;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages
{
    public class ReadMessageCommand: MessageCommand
    {
        public ReadMessageCommand(Message message, MessageConfig data, MessagesService messagesService)
            : base(message, data, messagesService)
        {

        }

        public override async Task<Message?> Execute()
        {
            if (this.prototype is Message message && this._messageConfig.UserId != null)
            {
                message = await this._messagesService.GetAsync(this._messageConfig.Id) ?? 
                          throw new NullReferenceException("There is no such message");

                if (message.CreatorId != this._messageConfig.UserId && !message.ReadBy.Contains(this._messageConfig.UserId))
                {
                    message.ReadBy.Add(this._messageConfig.UserId);
                    await this._messagesService.UpdateAsync(message.Id, message);
                    return message;
                }
                return null;
            }
            else return null;
        }
    }
}

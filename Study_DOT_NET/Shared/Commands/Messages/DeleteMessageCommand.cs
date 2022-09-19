using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages
{
    public class DeleteMessageCommand : MessageCommand
    {
        public DeleteMessageCommand(Message message, MessageConfig data, MessagesService messagesService)
            : base(message, data, messagesService)
        {

        }

        public override async Task<Message?> Execute()
        {
            if (this.prototype is Message message)
            {
                message.Id = this._messageConfig.Id;
                
                await this._messagesService.RemoveAsync(message.Id);
                return await this._messagesService.GetAsync(message.Id);
            }
            else return null;
        }
    }
}

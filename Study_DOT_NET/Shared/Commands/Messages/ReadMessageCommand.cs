using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Interfaces;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages
{
    public class ReadMessageCommand: MessageCommand
    {
        private string userId;
        public string UserId
        {
            get => this.userId;
            set
            {
                if (value.Length == 16)
                {
                    this.userId = value;
                }
            }
        }

        public ReadMessageCommand(Message message, MessageConfig data, MessagesService messagesService, string userId)
            : base(message, data, messagesService)
        {
            this.userId = userId;
        }

        public override async Task<Message?> Execute()
        {
            if (this.prototype is Message message && message.CreatorId != this.userId)
            {
                message = await this._messagesService.GetAsync(this._messageConfig.Id) ?? 
                          throw new ApplicationException("Wrong message ID or such message is not present in the data-base");

                if (message.CreatorId != this._messageConfig.UserId && !message.ReadBy.Contains(this._messageConfig.UserId))
                {
                    message.ReadBy?.Add(this.userId);
                    await this._messagesService.UpdateAsync(message.Id, message);
                    return message;
                }
                return null;
            }
            else return null;
        }
    }
}

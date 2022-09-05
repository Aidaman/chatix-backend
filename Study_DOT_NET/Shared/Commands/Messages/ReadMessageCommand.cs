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

        public override async Task Execute()
        {
            if (this.prototype is Message message && message.CreatorId != this.userId)
            {
                message.Id = this._messageConfig.Id;
                message.CreatorId = this._messageConfig.CreatorId;
                message.MessageContent = this._messageConfig.MessageContent;
                message.IsForwardedMessage = this._messageConfig.IsForwarded;

                message.ReadBy?.Add(this.userId);
                await this._messagesService.UpdateAsync(message.Id, message);
            }
        }
    }
}

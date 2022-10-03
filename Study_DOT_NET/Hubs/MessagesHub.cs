using Microsoft.AspNetCore.SignalR;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Commands.Messages;
using Study_DOT_NET.Shared.ConfigClasses;
using System.Text.Json;

namespace Study_DOT_NET.Hubs
{
    public class MessagesHub : Hub
    {
        private readonly CreateMessageCommand _createMessageCommand;
        private readonly DeleteMessageCommand _deleteMessageCommand;
        private readonly UpdateMessageCommand _updateMessageCommand;
        private readonly ReadMessageCommand _readMessageCommand;
        private readonly ILogger<Message> _logger;

        public MessagesHub(CreateMessageCommand createMessageCommand, DeleteMessageCommand deleteMessageCommand,
            UpdateMessageCommand updateMessageCommand, ReadMessageCommand readMessageCommand, ILogger<Message> logger)
        {
            this._createMessageCommand = createMessageCommand;
            this._deleteMessageCommand = deleteMessageCommand;
            this._updateMessageCommand = updateMessageCommand;
            this._readMessageCommand = readMessageCommand;
            this._logger = logger;
        }

        private MessageConfig GenerateMessageConfig(List<string> message)
        {
            return JsonSerializer.Deserialize<MessageConfig>(message[0]
                                                             ?? throw new NullReferenceException(
                                                                 "message parameter is null"))
                   ?? throw new NullReferenceException("unsuccessful deserialization result is null");
        }

        public async Task CreateMessage(List<string> message)
        {
            try
            {
                MessageConfig messageConfig = this.GenerateMessageConfig(message);

                this._createMessageCommand._messageConfig = messageConfig;
                Message? createdMessage = await this._createMessageCommand.Execute();

                await Clients.All.SendAsync("newMessage", createdMessage
                    ?? throw new ApplicationException("message prototype, occasionally, is not the message object"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, message);

                throw;
            }
        }

        public async Task UpdateMessage(List<string> message)
        {
            try
            {
                MessageConfig messageConfig = this.GenerateMessageConfig(message);

                this._updateMessageCommand._messageConfig = messageConfig;
                Message? updatedMessage = await this._updateMessageCommand.Execute();

                await Clients.All.SendAsync("messageUpdated", updatedMessage
                                                              ?? throw new ApplicationException(
                                                                  "message prototype, occasionally, is not the message object"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, message);
                throw;
            }
        }

        public async Task DeleteMessage(List<string> message)
        {
            try
            {
                MessageConfig messageConfig = this.GenerateMessageConfig(message);

                this._deleteMessageCommand._messageConfig = messageConfig;
                Message? deletedMessage = await this._deleteMessageCommand.Execute();

                await Clients.All.SendAsync("messageDeleted", deletedMessage
                                                              ?? throw new ApplicationException(
                                                                  "message prototype, occasionally, is not the message object"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, message);
                throw;
            }
        }

        public async Task ReadMessage(List<string> message)
        {
            try
            {
                MessageConfig messageConfig = this.GenerateMessageConfig(message);

                this._readMessageCommand._messageConfig = messageConfig;
                Message? updatedMessage = await this._readMessageCommand.Execute();

                await Clients.All.SendAsync("messageUpdated", updatedMessage
                    ?? throw new ApplicationException("message prototype, occasionally, is not the message object"));

                await Clients.All.SendAsync("messageRead", updatedMessage
                    ?? throw new ApplicationException("message prototype, occasionally, is not the message object"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, message);
                throw;
            }
        }
    }
}
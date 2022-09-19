using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Commands.Messages;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Hubs
{
    public class MessagesHub : Hub
    {
        private readonly CreateMessageCommand _createMessageCommand;
        private readonly DeleteMessageCommand _deleteMessageCommand;
        private readonly UpdateMessageCommand _updateMessageCommand;
        private readonly ReadMessageCommand _readMessageCommand;
        private readonly UsersService _usersService;
        private readonly MessagesService _messagesService;

        public MessagesHub(PrototypeRegistryService prototypeRegistryService, MessagesService messagesService,
            UsersService usersService)
        {
            Message? message = prototypeRegistryService.GetPrototypeById("message") as Message;

            this._usersService = usersService;
            this._messagesService = messagesService;

            this._createMessageCommand = new CreateMessageCommand(message!, new MessageConfig(), messagesService, usersService);
            this._updateMessageCommand = new UpdateMessageCommand(message!, new MessageConfig(), messagesService);
            this._readMessageCommand = new ReadMessageCommand(message!, new MessageConfig(), messagesService, "");
            this._deleteMessageCommand = new DeleteMessageCommand(message!, new MessageConfig(), messagesService);
        }

        public async Task CreateMessage(List<string> message)
        {
            try
            {
                MessageConfig messageConfig =
                    JsonSerializer.Deserialize<MessageConfig>(message[0]
                    ?? throw new NullReferenceException("message parameter is null"))
                    ?? throw new NullReferenceException("unsuccessful deserialization result is null");

                this._createMessageCommand._messageConfig = messageConfig;
                this._createMessageCommand._messageConfig!.Id = Guid.NewGuid().ToString("N").Substring(0, 24);
                Message? createdMessage = await this._createMessageCommand.Execute();
                await Clients.All.SendAsync("newMessage", JsonSerializer.Serialize<Message>(createdMessage
                    ?? throw new ApplicationException("message prototype, occasionally, is not the message object")));
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
                MessageConfig messageConfig =
                    JsonSerializer.Deserialize<MessageConfig>(message[0] 
                    ?? throw new NullReferenceException("message parameter is null"))
                    ?? throw new NullReferenceException("unsuccessful deserialization result is null");

                this._updateMessageCommand._messageConfig = messageConfig;
                Message? updatedMessage = await this._updateMessageCommand.Execute();

                await Clients.All.SendAsync("messageUpdated", JsonSerializer.Serialize<Message>(updatedMessage 
                    ?? throw new ApplicationException("message prototype, occasionally, is not the message object")));
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
                MessageConfig messageConfig =
                    JsonSerializer.Deserialize<MessageConfig>(message[0] 
                    ?? throw new NullReferenceException("message parameter is null"))
                    ?? throw new NullReferenceException("unsuccessful deserialization result is null");

                this._deleteMessageCommand._messageConfig = messageConfig;
                Message? deletedMessage = await this._deleteMessageCommand.Execute();

                await Clients.All.SendAsync("messageDeleted", JsonSerializer.Serialize<Message>(deletedMessage
                    ?? throw new ApplicationException("message prototype, occasionally, is not the message object")));
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
                MessageConfig messageConfig =
                    JsonSerializer.Deserialize<MessageConfig>(message[0]
                    ?? throw new NullReferenceException("message parameter is null"))
                    ?? throw new NullReferenceException("unsuccessful deserialization result is null");

                this._readMessageCommand._messageConfig = messageConfig;
                Message? updatedMessage = await this._readMessageCommand.Execute();

                Console.WriteLine($"updatedMessage: {updatedMessage}");

                await Clients.All.SendAsync("messageRead", JsonSerializer.Serialize<Message>(updatedMessage
                    ?? throw new ApplicationException("message prototype, occasionally, is not the message object")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, message);
                throw;
            }
        }
    }
}
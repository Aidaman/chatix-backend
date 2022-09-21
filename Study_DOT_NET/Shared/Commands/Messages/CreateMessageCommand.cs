using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages;

public class CreateMessageCommand: MessageCommand
{
    private readonly UsersService _usersService;

    public CreateMessageCommand(Message message, MessageConfig data, MessagesService messagesService, UsersService usersService) 
        : base(message, data, messagesService)
    {
        this._usersService = usersService;
    }

    public override async Task<Message?> Execute()
    {
        if (this.prototype is Message message)
        {
            message.Id = this._messageConfig.Id;
            message.MessageContent = this._messageConfig.MessageContent;
            message.IsForwardedMessage = this._messageConfig.IsForwarded;
            message.IsSystemMessage = this._messageConfig.IsSystem;
            message.RoomId = this._messageConfig.RoomId;
            message.CreatedAt = DateTime.Now;
            message.UpdatedAt = DateTime.Now;
            message.ReadBy = new List<string>();
            message.CreatorId = this._messageConfig.UserId;
            message.Creator = await this._usersService.GetAsync(message.CreatorId);

            await this._messagesService.CreateAsync(message);

            return message;
        }
        else return null;
    }
}
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages;

public class CreateMessageCommand : MessageCommand
{
    private readonly UsersService _usersService;

    public CreateMessageCommand(Message message, MessageConfig data, MessagesService messagesService,
        UsersService usersService)
        : base(message, data, messagesService)
    {
        this._usersService = usersService;
    }

    private async Task BuildMessage(Message message)
    {
        message.MessageContent = this._messageConfig.MessageContent;
        message.IsSystemMessage = this._messageConfig.IsSystem;
        message.RoomId = this._messageConfig.RoomId;
        message.CreatedAt = DateTime.Now;
        message.UpdatedAt = DateTime.Now;
        message.CreatorId = this._messageConfig.UserId;
    }

    public override async Task<Message?> Execute()
    {
        if (this.prototype.Clone() is Message message)
        {
            await this.BuildMessage(message);

            if (this._messageConfig.IsForwarded)
            {
                message = await this._messagesService.GetAsync(this._messageConfig.Id) ??
                          throw new NullReferenceException("There is no such message");

                message.IsForwardedMessage = true;
                message.Id = Guid.NewGuid().ToString("N").Substring(0, 24);
            }

            message.Creator = await this._usersService.GetAsync(message.CreatorId);
            message.ReadBy = new List<string>();

            await this._messagesService.CreateAsync(message);
            return message;
        }
        else return null;
    }
}
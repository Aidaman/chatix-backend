using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Messages;

public class UpdateMessageCommand : MessageCommand
{
    private readonly UsersService _usersService;
    public UpdateMessageCommand(PrototypeRegistryService prototypeRegistryService, MessagesService messagesService, 
        UsersService usersService)
        : base(prototypeRegistryService.GetPrototypeById("message") as Message, new MessageConfig(), messagesService)
    {
        _usersService = usersService;
    }

    public override async Task<Message?> Execute()
    {
        if (this.prototype.Clone() is Message message)
        {
            message = await this._messagesService.GetAsync(this._messageConfig.Id) ??
                      throw new NullReferenceException("Such message does not exist in the DB");
            message.MessageContent = this._messageConfig.MessageContent;
            
            await this._messagesService.UpdateAsync(message.Id, message);
            message.Creator = await this._usersService.GetAsync(message.CreatorId);
            
            return message;
        }
        else return null;
    }
}
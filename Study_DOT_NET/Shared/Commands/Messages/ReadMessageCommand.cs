using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Interfaces;
using Study_DOT_NET.Shared.Services;
using System.Linq;

namespace Study_DOT_NET.Shared.Commands.Messages
{
    public class ReadMessageCommand: MessageCommand
    {
        private readonly RoomsService _roomsService;
        private readonly UsersService _usersService;
        public ReadMessageCommand(Message message, MessageConfig data, MessagesService messagesService, RoomsService roomsService, UsersService usersService)
            : base(message, data, messagesService)
        {
            _roomsService = roomsService;
            _usersService = usersService;
        }

        public override async Task<Message?> Execute()
        {
            if (this.prototype.Clone() is Message message && this._messageConfig.UserId != null)
            {
                message = await this._messagesService.GetAsync(this._messageConfig.Id) ?? 
                          throw new NullReferenceException("There is no such message");
                User? creator = this._roomsService.Participants.FirstOrDefault((user) => user.Id == message.CreatorId, null);
                if (creator != null)
                {
                    message.Creator = creator;
                }
                else
                {
                    creator = await _usersService.GetAsync(message.CreatorId) ?? throw new NullReferenceException("There is no such User");
                    this._roomsService.Participants.Add(creator);
                    message.Creator = creator;
                }

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

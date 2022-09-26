using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Interfaces;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Users;

public class DisconectCommand : UsersCommand
{
    public DisconectCommand(User user, UserConfig data, UsersService usersService)
        : base(user, data, usersService)
    {

    }

    public override async Task<User?> Execute()
    {
        if (this.prototype.Clone() is User user)
        {
            user = await this._usersService.GetAsync(this._userConfig.Id) ?? throw new NullReferenceException("There is no such User");
            user.IsOnline = false;
            await this._usersService.UpdateAsync(user.Id, user);

            return user;
        }
        else return null;
    }
}
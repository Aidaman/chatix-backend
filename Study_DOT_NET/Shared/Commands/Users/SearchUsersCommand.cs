using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Interfaces;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Users;

public class SearchUsersCommand : UsersCommand
{
    public SearchUsersCommand(User user, UserConfig data, UsersService usersService)
        : base(user, data, usersService)
    {

    }

    public override async Task Execute()
    {
        if (this.prototype is User user)
        {
            user.Id = this._userConfig.Id;
            user.FullName = this._userConfig.Name;

            await this._usersService.SearchAsync(user.FullName);
        }
    }
}
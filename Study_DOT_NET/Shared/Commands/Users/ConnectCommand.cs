using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Interfaces;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Users;

public class ConnectCommand : UsersCommand
{
    public ConnectCommand(User user, UserConfig data, UsersService usersService)
        : base(user, data, usersService)
    {

    }

    public override Task Execute()
    {
        throw new NotImplementedException();

        if (this.prototype is User user)
        {
            user.Id = this._userConfig.Id;
            user.FullName = this._userConfig.Name;
            

        }
    }
}
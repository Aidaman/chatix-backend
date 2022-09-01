using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Interfaces;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Users;

public class ConnectCommand : Command
{
    private readonly UsersService _usersService;

    public ConnectCommand(User user, UserConfig data, UsersService usersService) : base(user)
    {
        ((this.prototype as User)!).Id = data.Id;
        ((this.prototype as User)!).FullName = data.Name;

        _usersService = usersService;
    }

    public override Task Execute()
    {
        throw new NotImplementedException();
    }
}
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Abstract;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.AbstractClasses;

public abstract class UsersCommand: Command
{
    protected readonly UsersService _usersService;
    public UserConfig _userConfig;

    protected UsersCommand(User user, UserConfig data, UsersService usersService) : base(user)
    {
        _usersService = usersService;
        _userConfig = data;
    }
}
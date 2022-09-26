using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Users;

public class ChangeThemeCommand: UsersCommand
{
    public ChangeThemeCommand(User user, UserConfig data, UsersService usersService)
        : base(user, data, usersService)
    {

    }

    public override async Task Execute()
    {
        if (this.prototype.Clone() is User user)
        {
            user.Id = this._userConfig.Id;
            user.FullName = this._userConfig.Name;

            user.ColorTheme = user.ColorTheme.ToLower() == "dark" ? "light" : "dark";
            await this._usersService.UpdateAsync(user._Id, user);
        }
    }
}

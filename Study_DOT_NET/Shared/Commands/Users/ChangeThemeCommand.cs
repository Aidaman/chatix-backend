using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.AbstractClasses;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Shared.Commands.Users;

public class ChangeThemeCommand: Command
{
    private readonly UsersService _usersService;

    public ChangeThemeCommand(User user, UserConfig data, UsersService usersService): base(user)
    {
        ((this.prototype as User)!).Id = data.Id;
        ((this.prototype as User)!).FullName = data.Name;

        _usersService = usersService;
    }

    public override async Task Execute()
    {
        User? user = this.prototype as User;
        if (user != null)
        {
            user.ColorTheme = user.ColorTheme.ToLower() == "dark" ? "light" : "dark";
            await this._usersService.UpdateAsync(user._Id, user);
        }
    }
}

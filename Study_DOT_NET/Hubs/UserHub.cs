﻿using Microsoft.AspNetCore.SignalR;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Hubs;

public class UserHub : Hub
{
    private readonly UserRegistryService _userRegistryService;
    private readonly UsersService _usersService;

    public UserHub(UsersService usersService, UserRegistryService userRegistryService)
    {
        _usersService = usersService;
        _userRegistryService = userRegistryService;
    }

    /// <summary>
    ///     Method by user once, when it connects/logins to the system
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="connectionId">
    ///     Connection id of the user, that should be used to send messages only to this user.
    ///     Does not work though ¯\_(ツ)_/¯.
    ///     this aspect should be fixed, but for now let hubs use Client.All
    /// </param>
    /// <exception cref="NullReferenceException">
    ///     Threw when:
    ///     1. the param itself is null
    ///     2. database does not have record of user with such id
    /// </exception>
    /// <exception cref="ApplicationException">Have no clue when it can be thrown</exception>
    public async Task Connect(string userId, string connectionId)
    {
        if (userId is null) throw new NullReferenceException("Can not connect empty user");

        var user = await _usersService.GetAsync(userId)
                   ?? throw new NullReferenceException("There is no such user");

        user.IsOnline = true;
        await _usersService.UpdateAsync(userId, user);

        _userRegistryService.AppendOne(connectionId, userId);

        Console.WriteLine($"Connected\n{_userRegistryService}");

        await Clients.All.SendAsync("connected", user
                                                 ?? throw new ApplicationException("Something went wrong"));
    }

    /// <summary>
    ///     Method by user once, when it being unconnected/logedout from the system
    /// </summary>
    /// <param name="userId">Id of user that is being disconnected</param>
    /// <exception cref="NullReferenceException">
    ///     Threw when:
    ///     1. the param itself is null
    ///     2. database does not have record of user with such id
    /// </exception>
    /// <exception cref="ApplicationException"> have no clue when it possibly can be thrown</exception>
    public async Task Disconnect(string userId)
    {
        if (userId is null) throw new NullReferenceException("Can not connect empty user");

        var user = await _usersService.GetAsync(userId)
                   ?? throw new NullReferenceException("There is no such user");
        user.IsOnline = false;
        await _usersService.UpdateAsync(userId, user);

        _userRegistryService.RemoveOne(userId);

        await Clients.All.SendAsync("disconnected", user
                                                    ?? throw new ApplicationException(
                                                        "room prototype, occasionally, is not the message object"));
    }
}
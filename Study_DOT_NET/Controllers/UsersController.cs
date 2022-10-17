using Microsoft.AspNetCore.Mvc;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Study_DOT_NET.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet]
    public async Task<List<User>> Get()
    {
        return await _usersService.GetAsync();
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null) return NotFound();

        return user;
    }

    [HttpGet("Search/{name}")]
    public async Task<ActionResult<List<User>>> SearchUsers(string name)
    {
        var users = await _usersService.SearchAsync(name);

        if (users.Count is 0) return NotFound();

        return users;
    }

    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
        await _usersService.CreateAsync(newUser);

        //CreatedAtAction() - creates ICreatedAtAction object that basically returns 201 response with Location header
        return CreatedAtAction(nameof(Get), new { id = newUser._Id }, newUser);
    }

    [HttpPut("id;length(24)")]
    public async Task<IActionResult> Update(string id, User updatedUser)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null) return NotFound();

        updatedUser._Id = user._Id;

        await _usersService.UpdateAsync(id, updatedUser);

        //NoContent() - creates NoContentResult object that basically returns response to server and signalling that there is empty response body
        return NoContent();
    }

    [HttpPut("ChangeTheme/id;length(24)")]
    public async Task<IActionResult> Update(string id)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null) return NotFound();

        user.ColorTheme = user.ColorTheme.ToLower() == "dark" ? "light" : "dark";
        await Update(id, user);

        return NoContent();
    }

    [HttpDelete("id;length(24)")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null) return NotFound();

        await _usersService.RemoveAsync(id);

        //NoContent() - creates NoContentResult object that basically returns response to server and signalling that there is empty response body
        return NoContent();
    }
}
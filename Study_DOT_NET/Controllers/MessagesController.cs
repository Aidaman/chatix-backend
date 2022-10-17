using Microsoft.AspNetCore.Mvc;
using Study_DOT_NET.Models;
using Study_DOT_NET.Shared.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Study_DOT_NET.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly MessagesService _messagesService;
    private readonly RoomsService _roomsService;
    private readonly UsersService _usersService;

    public MessagesController(MessagesService messagesService, UsersService usersService, RoomsService roomsService)
    {
        _messagesService = messagesService;
        _usersService = usersService;
        _roomsService = roomsService;
    }

    [HttpGet]
    public async Task<List<Message>> Get()
    {
        Console.WriteLine("GET messages");
        return await _messagesService.GetAsync();
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Message>> Get(string id)
    {
        Console.WriteLine($"GET message by ID: {id}");
        var message = await _messagesService.GetAsync(id);

        if (message is null) return NotFound();

        return message;
    }

    [HttpGet("RoomContent/{roomId:length(24)}/{offset}/{limit}")]
    public async Task<List<Message>?> Get(string roomId, int offset, int limit)
    {
        var messages = await _messagesService.GetRoomContentAsync(roomId, offset, limit);

        if (messages == null) return null;

        foreach (var message in messages)
        {
            /*
             if there is no such user in the List above then
             -> it search a user
             -> adds to list
             Why? Because making queries for EACH message, to get same users - are irrational
            */
            var creator =
                _roomsService.Participants.FirstOrDefault(user => user._Id == message.CreatorId, null);
            if (creator != null)
            {
                message.Creator = creator;
            }
            else
            {
                var user = await _usersService.GetAsync(message.CreatorId);
                _roomsService.Participants.Add(user);
                message.Creator = user;
            }
        }

        messages.Reverse();
        return messages;
    }

    [HttpGet("RoomAmountOfMessage/{roomId}")]
    public async Task<int> GetAmountOfMessages(string roomId)
    {
        if (roomId.ToLower() == "common") return 0;

        var result = await _messagesService.getAllMessagesFromRoom(roomId);

        if (result == null) return 0;

        return result.Count;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Message newMessage)
    {
        await _messagesService.CreateAsync(newMessage);

        //CreatedAtAction() - creates ICreatedAtAction object that basically returns 201 response with Location header
        return CreatedAtAction(nameof(Get), new { id = newMessage.Id }, newMessage);
    }

    [HttpPut("id:length(24)")]
    public async Task<IActionResult> Update(string id, Message updatedMessage)
    {
        var message = await _messagesService.GetAsync(id);

        if (message is null) return NotFound();

        updatedMessage.Id = message.Id;

        await _messagesService.UpdateAsync(id, updatedMessage);

        //NoContent() - creates NoContentResult object that basically returns response to server and signalling that there is empty response body
        return NoContent();
    }

    [HttpDelete("id:length(24)")]
    public async Task<IActionResult> Delete(string id)
    {
        var message = await _messagesService.GetAsync(id);

        if (message is null) return NotFound();

        await _messagesService.RemoveAsync(id);

        //NoContent() - creates NoContentResult object that basically returns response to server and signalling that there is empty response body
        return NoContent();
    }
}